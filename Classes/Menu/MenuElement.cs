using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Menus;

public abstract class MenuElement
{
	public bool IsSelected { get; set; } = false;

	public Vector2 Size { get; set; }

	public MenuPivot Pivot { get; set; }
	public Vector2 Offset { get; set; }
	public Vector2 Position
	{
		get
		{
			Vector2 half = Main.WindowSize / 2f;
			return Pivot switch
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
			} + Main.WindowCenter + Offset;
		}
	}

	public abstract void Start(ContentManager content);
	public abstract void Update();
	public abstract void Draw(SpriteBatch spriteBatch);
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