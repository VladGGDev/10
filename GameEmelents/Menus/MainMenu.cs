using Menus;
using Menus.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class MainMenu : Menu
{
	MenuGroup _mainMenu;
	MenuGroup _settings;
	Image _background;

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
		_background = new()
		{
			Texture = Main.Pixel,
			Size = Main.WindowSize,
			Color = Color.Black,
			LayerDepth = 0
		};

		_mainMenu = new(playButton);
		_mainMenu.AddNode(gameTitle);
		_mainMenu.AddNode(_background);

		_mainMenu.AddNode(settingsButton);
		_mainMenu.AddRightLink(playButton, settingsButton);

		_mainMenu.AddNode(exitButton);
		_mainMenu.AddLeftLink(playButton, exitButton);



		// ===== Settings =====
		const float settingsStartY = -250f;
		const float settingsButtonDistance = 150f;
		const float settingsButtonSize = 0.25f;
		Slider soundSlider = new()
		{
			Texture = content.Load<Texture2D>("Icons/Sound"),
			Font = content.Load<SpriteFont>("Fonts/Roboto-Light"),
			Offset = new(7, settingsStartY + settingsButtonDistance),
			Size = new(settingsButtonSize),

			Value = 0.8f, // Change to get from save file
			OnValueChanged = (volume) => SoundEffect.MasterVolume = volume
		};
		float settingsButtonOffset = -soundSlider.GetSliderDimensions().X / 2f + 512f / 2f * settingsButtonSize;
		Button backButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Back"),
			Offset = new(settingsButtonOffset, settingsStartY),
			Size = new(settingsButtonSize),

			OnInteract = () => CurrentGroup = _mainMenu
		};
		// Fullscreen toggle and logic
		Button fullscreenToggle = new()
		{
			Texture = content.Load<Texture2D>("Icons/Toggle Fullscreen"),
			Offset = new(settingsButtonOffset, settingsStartY + settingsButtonDistance * 2f),
			Size = new(settingsButtonSize)
		};
		Button deleteSaveButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Delete save"),
			Offset = new(settingsButtonOffset, settingsStartY + settingsButtonDistance * 3f),
			Size = new(settingsButtonSize)
		};

		_settings = new(backButton);

		_settings.AddNode(soundSlider);
		_settings.AddNode(fullscreenToggle);
		_settings.AddNode(deleteSaveButton);
		_settings.AddNode(_background);

		_settings.AddDownLink(backButton, soundSlider);
		_settings.AddDownLink(soundSlider, fullscreenToggle);
		_settings.AddDownLink(fullscreenToggle, deleteSaveButton);

		_mainMenu.Start(content);
		_settings.Start(content);
		CurrentGroup = _mainMenu;
	}

	public override void Update()
	{
		_background.Size = Main.WindowSize;
		if (CurrentGroup == _settings && Input.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			CurrentGroup = _mainMenu;

		base.Update();
	}
}
