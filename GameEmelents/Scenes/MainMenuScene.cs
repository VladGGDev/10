using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class MainMenuScene : Scene
{
	MainMenu _mainMenu;

	public MainMenuScene() : base(null)
	{
		_mainMenu = new();
	}

	public override void Start(ContentManager content)
	{
		_mainMenu.Start(content);
		base.Start(content);
	}

	public override void Update()
	{
		_mainMenu.Update();
		base.Update();
	}

	public override void Draw(SpriteBatch spriteBatch, ExampleRenderer levelRenderer)
	{
		_mainMenu.Draw(spriteBatch);
		base.Draw(spriteBatch, levelRenderer);
	}
}
