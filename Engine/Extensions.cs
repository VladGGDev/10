using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public static class ColliderExtensions
{
	public static void AddToActor(this Collider collider, Actor actor)
	{
		actor.Collider = collider;
		collider.Actor = actor;
	}
}


public static class SpriteBatchExtensions
{
	public static void DrawSquareOutline(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
		=> DrawSquareOutline(spriteBatch, position.ToPoint(), size.ToPoint(), color);

	public static void DrawSquareOutline(this SpriteBatch spriteBatch, Point position, Point size, Color color)
	{
		Point halfSize = new Point(size.X / 2, size.Y / 2);

		// Up
		spriteBatch.Draw(
			Main.Pixel, 
			new Rectangle(position.X - halfSize.X, position.Y - halfSize.Y, size.X, 1),
			null,
			color,
			0,
			Vector2.Zero,
			SpriteEffects.None,
			1f);
		// Down
		spriteBatch.Draw(
			Main.Pixel,
			new Rectangle(position.X - halfSize.X, position.Y + halfSize.Y, size.X, 1),
			null,
			color,
			0,
			Vector2.Zero,
			SpriteEffects.None,
			1f);
		// Right
		spriteBatch.Draw(
			Main.Pixel,
			new Rectangle(position.X + halfSize.X, position.Y - halfSize.Y, 1, size.Y),
			null,
			color,
			0,
			Vector2.Zero,
			SpriteEffects.None,
			1f);
		// Left
		spriteBatch.Draw(
			Main.Pixel,
			new Rectangle(position.X - halfSize.X, position.Y - halfSize.Y, 1, size.Y),
			null,
			color,
			0,
			Vector2.Zero,
			SpriteEffects.None,
			1f);
	}

	public static void DrawCircleOutline(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
	{
		float resolution = MathF.Tau * radius; // length of circumference = 2*PI*R
		for (int i = 0; i < resolution; i++)
		{
			float rad = MathF.Tau / resolution * i;
			Vector2 newPos = position;
			newPos.X += MathF.Cos(rad) * radius;
			newPos.Y += MathF.Sin(rad) * radius;
			
			spriteBatch.Draw(
				Main.Pixel,
				new Rectangle(newPos.ToPoint(), new Point(1)),
				null,
				color,
				0,
				Vector2.Zero,
				SpriteEffects.None,
				1f);
		}
	}

	public static void DrawSimlple(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float rotation, Vector2 Size, Color color, float depth)
	{
		spriteBatch.Draw(
			texture,
			position,
			null,
			color,
			rotation,
			texture.Bounds.Center.ToVector2(),
			Size,
			SpriteEffects.None,
			depth);
	}
}


public static class LerpExtensions
{
	public static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	public static float InverseLerp(float a, float b, float val)
	{
		return (val - a) / (b - a);
	}

	public static float Remap(float aIn, float bIn, float aOut, float bOut, float val)
	{
		return Lerp(aOut, bOut, InverseLerp(aIn, bIn, val));
	}
}