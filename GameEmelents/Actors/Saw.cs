using Microsoft.Xna.Framework;
using LDtk;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Tweening;

internal class Saw : Actor
{
	// LDtk
	public bool IsYoYo { get; set; }
	public bool IsEased { get; set; }
	public float[] Speed { get; set; }
	public float[] WaitTime { get; set; }
	public Vector2[] Points { get; set; }
	TweenSequence _tween;

	Texture2D _texture;

	public const float RotateSpeed = MathF.Tau * 1f;
	public const float ColliderSizeMultiplier = 0.75f;

	public override void Start(ContentManager content)
	{
		_texture = content.Load<Texture2D>("Sawblade");
		this.AddCollider(new CircleCollider(Position, Size.X / 2f * ColliderSizeMultiplier, "Spike"));

		_tween = new();
	}

	public override void Update()
	{

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
}