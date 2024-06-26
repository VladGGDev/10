using Microsoft.Xna.Framework;
using System.Collections.Generic;

public abstract class Collider
{
	public static List<Collider> Colliders = new List<Collider>();
	public string Tag { get; set; }
	public Actor Actor { get; set; }
	public Vector2 Position { get; set; }

	public Collider(Vector2 position, string tag) : this()
	{
		Position = position;
		Tag = tag;
	}

	public Collider(Vector2 position) : this(position, "")
	{
	}

	public Collider()
	{
		Colliders.Add(this);
	}

	public Vector2 GetRelativePosition(Vector2 other) => other - Position;
	public Vector2 GetRelativePosition(Collider other) => other.Position - Position;
	public abstract bool Check(Collider other);
	public abstract Vector2 ClosestPointOnBounds(Vector2 position);
}