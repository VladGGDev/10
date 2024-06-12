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
}
