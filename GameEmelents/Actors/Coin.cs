using Microsoft.Xna.Framework;
using LDtk;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

internal class Coin : Actor
{
	public static int Coins = 0;

	public float ColliderRadius = 100f;

	public float Magnitude = 8f;
	public float Frequency = 2 * MathF.PI * 0.5f;
	public float PositionInfluence = 0.1f;
	Vector2 _offset;

	public override void Start(ContentManager content)
	{
		AddCollider(new CircleCollider(Position, Size.X / 2f, "Coin"));
		//Collider = new CircleCollider(Position, Size.X / 2f, "Coin");
		Coins++;
	}

	public override void Update()
	{
		_offset.Y = MathF.Sin((Main.TotalTime + Position.X * PositionInfluence) * Frequency) * Magnitude;
		//_offset.X = MathF.Sin((Main.TotalTime + 10f) * Frequency * 0.7123f) * Magnitude;
		Collider.Position = Position + _offset;
	}

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes[""];
		pass.Draw(
			Main.Pixel, 
			Position + _offset, 
			null, 
			Color.Gold, 
			MathF.PI * 0.25f, 
			Pivot,
			Size, 
			SpriteEffects.None, 
			0);
		
		if (Main.DebugGraphics)
			pass.DrawCircleOutline(Collider.Position, Size.X / 2f, Color.DarkOrange);
	}

	public override void End()
	{
		Coins = 0;
	}
}