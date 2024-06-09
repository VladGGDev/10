using Microsoft.Xna.Framework;
using LDtk;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

internal class Coin : Actor
{
	Texture2D _texture;

	public static int Coins = 0;

	public float ColliderRadius = 100f;

	public float Magnitude = 8f;
	public float Frequency = 2 * MathF.PI * 0.5f;
	public float PositionInfluence = 0.1f;
	Vector2 _offset;

	public override void Start(ContentManager content)
	{
		_texture = content.Load<Texture2D>("Square");

		this.AddCollider(new CircleCollider(Position, Size.X / 2f, "Coin"));
		//Collider = new CircleCollider(Position, Size.X / 2f, "Coin");
		Coins++;
	}

	public override void Update()
	{
		Main.DebugMessage = Coins.ToString();
		_offset.Y = MathF.Sin((Main.TotalTime + Position.X * PositionInfluence) * Frequency) * Magnitude;
		//_offset.X = MathF.Sin((Main.TotalTime + 10f) * Frequency * 0.7123f) * Magnitude;
		Collider.Position = Position + _offset;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(
			_texture, 
			Position + _offset, 
			null, 
			Color.Gold, 
			MathF.PI * 0.25f, 
			Pivot, Size, 
			SpriteEffects.None, 
			0);
		// Collider 
		//spriteBatch.Draw(_texture, Collider.Position, null, new Color(1f, 0, 0, 0.2f), 0, Pivot, Size, SpriteEffects.None, 0.1f);
		spriteBatch.DrawSquareOutline(Collider.Position, Size, Color.Orange);
	}

	public override void End()
	{
		Coins = 0;
	}
}