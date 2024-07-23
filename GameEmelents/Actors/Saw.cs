using Microsoft.Xna.Framework;
using LDtk;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Tweening;
using Microsoft.Xna.Framework.Audio;

internal class Saw : Actor
{
	// LDtk
	public bool IsYoYo { get; set; }
	public bool IsEased { get; set; }
	public float[] Speed { get; set; }
	public float[] WaitTime { get; set; }
	public Vector2[] Points { get; set; }
	TweenSequence _sequence;
	public float StartMovingTime { get; set; }

	Texture2D _texture;

	// Sound
	SoundEffectInstance _soundEffect;
	public const float PanningExponent = 3f;
	public const float SoundRadius = 750f;
	public const float SoundFalloffExponent = 2f;
	public const float MaxVolume = 0.7f;


	public const float RotateSpeed = MathF.Tau * 1f;
	public const float ColliderSizeMultiplier = 0.75f;
	Countdown _countdown;

	public override void Start(ContentManager content)
	{
		_texture = content.Load<Texture2D>("Sawblade");
		_soundEffect = content.Load<SoundEffect>("Sounds/Saw").CreateInstance();
		_soundEffect.IsLooped = true;
		_soundEffect.Volume = 0;
		_soundEffect.Play();

		AddCollider(new CircleCollider(Position, Size.X / 2f * ColliderSizeMultiplier, "Spike"));

		_countdown = SceneManager.CurrentScene.GetActor<Countdown>();

		if (Points.Length > 0)
		{
			// Offset each point by EntityLayerOffset
			for (int i = 0; i < Points.Length; i++)
				Points[i] += Main.EntityLayerOffset;
			_sequence = new();

			// First
			_sequence.Add(new Tween(Position, Points[0], Vector2.Distance(Points[0], Position) / Speed[0]));
			if (WaitTime[0] > 0)
				_sequence.AddDelay(WaitTime[0]);
			// The rest
			for (int i = 1; i < Points.Length; i++)
			{
				float speed = Speed.Length == 1 ? Speed[0] : Speed[i];
				float wait = WaitTime.Length == 1 ? WaitTime[0] : WaitTime[i];

				_sequence.Add(new Tween(Points[i - 1], Points[i], Vector2.Distance(Points[i - 1], Points[i]) / speed));
				if (wait > 0)
					_sequence.AddDelay(wait);
			}
			// Yo-Yo
			if (!IsYoYo)
				return;
			for (int i = Points.Length - 1; i > 0; i--)
			{
				float speed = Speed.Length == 1 ? Speed[0] : Speed[i];
				float wait = WaitTime.Length == 1 ? WaitTime[0] : WaitTime[i];

				_sequence.Add(new Tween(Points[i], Points[i - 1], Vector2.Distance(Points[i], Points[i - 1]) / speed));
				if (wait > 0)
					_sequence.AddDelay(wait);
			}
			// Yo-Yo last
			_sequence.Add(new Tween(Points[0], Position, Vector2.Distance(Points[0], Position) / Speed[0]));
			if (WaitTime[0] > 0)
				_sequence.AddDelay(WaitTime[0]);

			_sequence.Restart();
		}
	}

	public override void Update()
	{
		// Sounds
		float pan = Vector2.Normalize(Collider.Position - Camera.Instance.Position).X;
		_soundEffect.Pan = MathF.Pow(pan, PanningExponent);
		float volume = Math.Clamp(1f - Vector2.Distance(Collider.ClosestPointOnBounds(Camera.Instance.Position), Camera.Instance.Position) / SoundRadius, 0, 1f);
		_soundEffect.Volume = Math.Min(MathF.Pow(volume, SoundFalloffExponent), MaxVolume);
		//Main.DebugMessage = _soundEffect.Volume.ToString("F4");

		// Moving
		if (Points.Length == 0)
			return;
		if (_countdown.Time > StartMovingTime)
			_sequence.Restart();

		Collider.Position = _sequence.Result();
		if (_sequence.ElapsedTime > _sequence.TotalDuration)
			_sequence.Restart();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(
			_texture,
			Collider.Position,
			null,
			new Color(255, 25, 25),
			Main.TotalTime * RotateSpeed,
			_texture.Bounds.Center.ToVector2(),
			Size / _texture.Height,
			SpriteEffects.None,
			0.91f);
		spriteBatch.DrawSquareOutline(Collider.Position, Size * ColliderSizeMultiplier, Color.Black);
	}

	public override void End()
	{
		_soundEffect.Dispose();
	}
}