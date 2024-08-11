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
	Player _player;

	// Sound
	SoundEffectInstance _soundEffect;
	public const float PanningExponent = 3f;
	public const float SoundRadius = 750f;
	public const float SoundFalloffExponent = 2f;
	public const float MaxVolume = 0.5f;


	public const float RotateSpeed = MathF.Tau * 1f;
	public const float ColliderSizeMultiplier = 0.75f;
	Countdown _countdown;

	public override void Start(ContentManager content)
	{
		_player = SceneManager.CurrentScene.GetActor<Player>();
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
			AddToSequence(Position, Points[0], Speed[0], WaitTime[0]);
			// The rest
			for (int i = 1; i < Points.Length; i++)
			{
				float speed = Speed.Length == 1 ? Speed[0] : Speed[i];
				float wait = WaitTime.Length == 1 ? WaitTime[0] : WaitTime[i];

				AddToSequence(Points[i - 1], Points[i], speed, wait);
			}
			// Yo-Yo
			if (!IsYoYo)
				return;
			for (int i = Points.Length - 1; i > 0; i--)
			{
				float speed = Speed.Length == 1 ? Speed[0] : Speed[i];
				float wait = WaitTime.Length == 1 ? WaitTime[0] : WaitTime[i];

				AddToSequence(Points[i], Points[i - 1], speed, wait);
			}
			// Yo-Yo last
			AddToSequence(Points[0], Position, Speed[0], WaitTime[0]);

			_sequence.Restart();
		}
	}

	void AddToSequence(Vector2 from, Vector2 to, float speed, float waitTime)
	{
		Tween tween = new Tween(from, to, Vector2.Distance(from, to) / speed);
		if (IsEased)
			tween.SetEasing(EasingFunctions.EaseInOutCubic);
		_sequence.Add(tween);
		if (waitTime > 0)
			_sequence.AddDelay(waitTime);
	}

	public override void Update()
	{
		// Sounds
		float pan = Vector2.Normalize(Collider.Position - _player.Collider.Position).X;
		_soundEffect.Pan = MathF.Pow(pan, PanningExponent);
		float volume = Math.Clamp(1f - Vector2.Distance(Collider.ClosestPointOnBounds(_player.Collider.Position), _player.Collider.Position) / SoundRadius, 0, 1f);
		volume = MathF.Pow(volume, SoundFalloffExponent);
		_soundEffect.Volume = LerpExtensions.Remap(0, 1f, 0, MaxVolume, volume);
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

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes[""];
		pass.Draw(
			_texture,
			Collider.Position,
			null,
			Color.White,
			Main.TotalTime * RotateSpeed,
			_texture.Bounds.Center.ToVector2(),
			Size / _texture.Height,
			SpriteEffects.None,
			0.2f);

		if (Main.DebugGraphics)
			pass.DrawCircleOutline(Collider.Position, Size.X / 2 * ColliderSizeMultiplier, Color.Black);
	}

	public override void End()
	{
		_soundEffect.Dispose();
	}
}