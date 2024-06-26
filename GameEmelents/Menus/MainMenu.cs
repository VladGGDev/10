using Menus;
using Menus.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class MainMenu : Menu
{
	MenuGroup _mainMenu;
	MenuGroup _settings;

	public override void Start(ContentManager content)
	{
		// ===== Main Menu =====
		const float buttonDistance = 512f;
		const float buttonY = 150f;
		const float bigButtonSize = 0.7f;
		const float buttonSize = 0.6f;

		Button playButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Play"),
			Offset = new(0, buttonY),
			Size = new(bigButtonSize),
			OnInteract = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1)
		};
		Button settingsButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Settings"),
			Offset = new(buttonDistance, buttonY),
			Size = new(buttonSize),
			OnInteract = () => CurrentGroup = _settings
		};
		Button exitButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Exit"),
			Offset = new(-buttonDistance, buttonY),
			Size = new(buttonSize),
			OnInteract = () => Main.Instance.Exit()
		};
		Text gameTitle = new()
		{
			Font = content.Load<SpriteFont>("Fonts/Roboto-Light"),
			String = "10",
			Offset = new(0, -300f),
			Size = new(4f)
		};

		_mainMenu = new(playButton);
		_mainMenu.AddNode(gameTitle);

		_mainMenu.AddNode(settingsButton);
		_mainMenu.AddRightLink(playButton, settingsButton);

		_mainMenu.AddNode(exitButton);
		_mainMenu.AddLeftLink(playButton, exitButton);



		// ===== Settings =====
		Image settingsImg = new()
		{
			Texture = content.Load<Texture2D>("Sawblade"),
			OnSelected = () => { } // No error
		};

		_settings = new(settingsImg);


		CurrentGroup = _mainMenu;
		CurrentGroup.Start(content);
	}

	public override void Update()
	{
		if (CurrentGroup == _settings && Input.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			CurrentGroup = _mainMenu;

		base.Update();
	}
}
