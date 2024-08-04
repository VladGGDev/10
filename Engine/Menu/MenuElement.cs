using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Menus;

public abstract class MenuElement
{
	public bool IsSelected { get; set; } = false;
	public Action OnSelected { get; set; }
	public Action OnDeselected { get; set; }

	public Vector2 Size { get; set; } = Vector2.One;
	float _layerDepth = 0.91f;
	public float LayerDepth 
	{ 
		get => _layerDepth; 
		set => _layerDepth = MathHelper.Clamp(value, 0, 1f); 
	}

	public MenuPivot ScreenPivot { get; set; } = MenuPivot.Center;
	public Vector2 Offset { get; set; } = Vector2.Zero;
	public Vector2 Position
	{
		get
		{
			Vector2 half = Main.WindowSize / 2f;
			return ScreenPivot switch
			{
				MenuPivot.Center => Vector2.Zero,
				MenuPivot.Up => new Vector2(0, -half.Y),
				MenuPivot.Down => new Vector2(0, half.Y),
				MenuPivot.Right => new Vector2(half.X, 0),
				MenuPivot.Left => new Vector2(-half.X, 0),

				MenuPivot.UpLeft => -half,
				MenuPivot.UpRight => new Vector2(half.X, -half.Y),
				MenuPivot.DownRight => half,
				MenuPivot.DownLeft => new Vector2(-half.X, half.Y),
				_ => Vector2.Zero
			} + half + Offset;
		}
	}
	public Vector2 Pivot => ScreenPivot switch
			{
				MenuPivot.Center => new Vector2(0.5f),
				MenuPivot.Up => new Vector2(0.5f, 0),
				MenuPivot.Down => new Vector2(0.5f, 1f),
				MenuPivot.Right => new Vector2(1f, 0.5f),
				MenuPivot.Left => new Vector2(0, 0.5f),

				MenuPivot.UpLeft => Vector2.Zero,
				MenuPivot.UpRight => new Vector2(1f, 0),
				MenuPivot.DownRight => Vector2.One,
				MenuPivot.DownLeft => new Vector2(0, 1f),
				_ => new Vector2(0.5f)
			};

	public abstract void Start(ContentManager content);
	public virtual void Update() { }
	public abstract void Draw();
}

public enum MenuPivot
{
	Center,
	Up,
	Down,
	Right,
	Left,
	UpLeft,
	UpRight,
	DownRight,
	DownLeft,
}