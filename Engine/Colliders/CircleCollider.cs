using Microsoft.Xna.Framework;
using System;

public class CircleCollider : Collider
{
	public float Radius { get; set; }

	public CircleCollider(Vector2 position, float radius, string tag) : base(position, tag)
	{
		Radius = radius;
	}

	public CircleCollider(Vector2 position, float radius) : this(position, radius, "")
	{
	}


	public override bool Check(Collider other)
	{
		if (other is BoxCollider)
		{
			return other.Check(this);
		}
		else if (other is CircleCollider otherCircleCol)
		{
			float distanceSquared = Vector2.DistanceSquared(Position, otherCircleCol.Position);
			return distanceSquared < (Radius + otherCircleCol.Radius) * (Radius + otherCircleCol.Radius);
		}
		return false;
	}
}