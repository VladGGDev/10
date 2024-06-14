using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;


public class Camera
{
	public static Camera Instance { get; private set; }

	public Vector2 Position { get; set; }
	public float Size { get; set; }

	public Vector2 Dimensions => new(Size * Main.TargetAspectRatio, Size);

	public Camera(Vector2 position, float size)
	{
		Instance = this;
		
		Position = position;
		Size = size;
	}
}
