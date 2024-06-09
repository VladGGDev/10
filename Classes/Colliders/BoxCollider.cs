using Microsoft.Xna.Framework;
using System;

public class BoxCollider : Collider
{
	public Vector2 Size { get; set; }

	Vector2 HalfSize => Size * 0.5f;

	public BoxCollider(Vector2 position, Vector2 size, string tag) : base(position, tag)
	{
		Size = size;
	}

	public BoxCollider(Vector2 position, Vector2 size) : this(position, size, "")
	{
	}


	public override bool Check(Collider other)
	{
		if (other is BoxCollider otherBoxCol)
		{
			return Math.Abs(Position.X - otherBoxCol.Position.X) * 2  <  Size.X + otherBoxCol.Size.X &&
				   Math.Abs(Position.Y - otherBoxCol.Position.Y) * 2  <  Size.Y + otherBoxCol.Size.Y;
		}
		else if (other is CircleCollider otherCircleCol)
		{
			Vector2 relPos = GetRelativePosition(otherCircleCol);
			relPos = new(MathF.Abs(relPos.X), MathF.Abs(relPos.Y));

			// Check if too far
			if (relPos.X > otherCircleCol.Radius + HalfSize.X || relPos.Y > otherCircleCol.Radius + HalfSize.Y)
				return false;

			// Check if too close
			if (relPos.X <= HalfSize.X || relPos.Y <= HalfSize.Y)
				return true;

			// Check corners
			float cornerDistSq = Vector2.DistanceSquared(relPos, HalfSize);

			return cornerDistSq <= otherCircleCol.Radius * otherCircleCol.Radius;
		}
		return false;
	}
}