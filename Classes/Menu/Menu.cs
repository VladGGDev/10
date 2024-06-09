using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Menus;

public abstract class Menu
{
	public MenuGroup CurrentGroup { get; set; }

	public virtual void Start(ContentManager content)
	{
		CurrentGroup.Start(content);
	}

	public virtual void Update()
	{
		CurrentGroup.Update();
	}

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		CurrentGroup.Draw(spriteBatch);
	}



#region Input handling
	static readonly Keys[] _interactKeys = { Keys.Space, Keys.W, Keys.Up };
	static readonly Keys[] _leftKeys = { Keys.A, Keys.Left };
	static readonly Keys[] _rightKeys = { Keys.D, Keys.Right };
	static readonly Keys[] _upKeys = { Keys.W, Keys.Up };
	static readonly Keys[] _downKeys = { Keys.S, Keys.Down };

	public static bool GetAnyRightKeyDown()
	{
		for (int i = 0; i < _rightKeys.Length; i++)
			if (Input.GetKeyDown(_rightKeys[i]))
				return true;
		return false;
	}

	public static bool GetAnyLeftKeyDown()
	{
		for (int i = 0; i < _leftKeys.Length; i++)
			if (Input.GetKeyDown(_leftKeys[i]))
				return true;
		return false;
	}

	public static bool GetAnyUpKeyDown()
	{
		for (int i = 0; i < _upKeys.Length; i++)
			if (Input.GetKeyDown(_upKeys[i]))
				return true;
		return false;
	}

	public static bool GetAnyDownKeyDown()
	{
		for (int i = 0; i < _downKeys.Length; i++)
			if (Input.GetKeyDown(_downKeys[i]))
				return true;
		return false;
	}

	public static bool GetAnyInteractKeyDown()
	{
		for (int i = 0; i < _interactKeys.Length; i++)
			if (Input.GetKeyDown(_interactKeys[i]))
				return true;
		return false;
	}
#endregion
}
