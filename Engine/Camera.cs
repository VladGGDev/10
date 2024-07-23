using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;


public class Camera
{
	public static Camera Instance { get; private set; }

	Vector2 _position;
	public Vector2 Position 
	{
		get => _position;
		set => _position = ClosestPointInsideBounds(value);
	}
	public float Size { get; set; }
	public Rectangle? Bounds { get; set; }
	public Vector2 Dimensions => new(Size * Main.TargetAspectRatio, Size);

	public Camera(Vector2 position, float size, Rectangle? bounds = null)
	{
		Instance = this;
		
		Position = position;
		Size = size;
		Bounds = bounds;
	}


	public bool PositionInBounds(Vector2 position)
	{
		if (!Bounds.HasValue)
			return true;

		return Bounds.Value.Contains(new Rectangle(position.ToPoint(), Dimensions.ToPoint()));
	}

	public Vector2 ClosestPointInsideBounds(Vector2 position)
	{
		if (!Bounds.HasValue)
			return position;

		Vector2 boundsSize = new Vector2(Bounds.Value.Width, Bounds.Value.Height);
		Vector2 boundsPos = Bounds.Value.Center.ToVector2();// new Vector2(Bounds.Value.X, Bounds.Value.Y);

		Vector2 clampVector = (boundsSize - Dimensions) * 0.5f;
		Vector2 relativePos = position - boundsPos;
		Vector2 clampedPos = Vector2.Clamp(relativePos, -clampVector, clampVector);

		position += clampedPos - relativePos;

		if (Dimensions.X > boundsSize.X)
			position.X = boundsPos.X;
		if (Dimensions.Y > boundsSize.Y)
			position.Y = boundsPos.Y;

		return position;
	}
}
