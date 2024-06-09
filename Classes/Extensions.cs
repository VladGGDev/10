using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class ColliderExtensions
{
	public static void AddCollider(this Actor actor, Collider collider)
	{
		actor.Collider = collider;
		collider.Actor = actor;
	}

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
		spriteBatch.Draw(Main.Pixel, new Rectangle(position.X - halfSize.X, position.Y - halfSize.Y, size.X, 1), color);
		// Down
		spriteBatch.Draw(Main.Pixel, new Rectangle(position.X - halfSize.X, position.Y + halfSize.Y, size.X, 1), color);
		// Right
		spriteBatch.Draw(Main.Pixel, new Rectangle(position.X + halfSize.X, position.Y - halfSize.Y, 1, size.Y), color);
		// Left
		spriteBatch.Draw(Main.Pixel, new Rectangle(position.X - halfSize.X, position.Y - halfSize.Y, 1, size.Y), color);
	}
}