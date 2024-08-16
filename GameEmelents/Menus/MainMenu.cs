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

	Fade _fade = new()
	{
		FadeInTime = 0.15f,
		FadeStayTime = 0f,
		FadeOutTime = 0.15f
	};

	public override void Start(ContentManager content)
	{
		// ===== Main Menu =====
		const float buttonDistance = 512f;
		const float buttonY = 250f;
		const float bigButtonSize = 0.7f;
		const float buttonSize = 0.6f;

		Button playButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Play"),
			Offset = new(0, buttonY),
			Size = new(bigButtonSize),
			OnInteract = () =>
			{
				_fade.FadeInTime = 0.25f;
				_fade.OnFaded = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1);
				_fade.StartFading();
			}
		};
		Button settingsButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Settings"),
			Offset = new(buttonDistance, buttonY),
			Size = new(buttonSize),
			OnInteract = () => 
			{
				_fade.FadeInTime = 0.15f;
				_fade.FadeOutTime = 0.15f;
				_fade.OnFaded = () => CurrentGroup = _settings;
				_fade.StartFading();
			}
		};
		Button exitButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Exit"),
			Offset = new(-buttonDistance, buttonY),
			Size = new(buttonSize),
			OnInteract = () =>
			{
				_fade.FadeInTime = 0.35f;
				_fade.OnFaded = () => Main.Instance.Exit();
				_fade.StartFading();
			}
		};
		Text gameTitle = new()
		{
			Font = content.Load<SpriteFont>("Fonts/Roboto-Medium"),
			String = "10",
			Offset = new(0, -250f),
			Size = new(0.45f)
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
		const float settingsStartY = -200f;
		const float settingsButtonDistance = 150f;
		const float settingsButtonSize = 0.25f;
		Slider soundSlider = new()
		{
			Texture = content.Load<Texture2D>("Icons/Sound"),
			Font = content.Load<SpriteFont>("Fonts/Roboto-Regular"),
			Offset = new(7, settingsStartY + settingsButtonDistance),
			Size = new(settingsButtonSize),

			Value = 0.8f, // Change to get from save file
			OnValueChanged = (volume) => SoundEffect.MasterVolume = volume
		};
		float settingsButtonOffset = -soundSlider.GetSliderDimensions().X / 2f + 512f / 2f * settingsButtonSize;
		Button backButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Back"),
			Offset = new(settingsButtonOffset, settingsStartY - 75),
			Size = new(settingsButtonSize),

			OnInteract = () =>
			{
				_fade.OnFaded = () => CurrentGroup = _mainMenu;
				_fade.StartFading();
			}
		};
		// Fullscreen toggle and logic
		Button fullscreenToggle = new()
		{
			Texture = content.Load<Texture2D>("Icons/Toggle Fullscreen"),
			Offset = new(settingsButtonOffset, settingsStartY + settingsButtonDistance * 2f),
			Size = new(settingsButtonSize),

			OnInteract = () => Main.IsFullscreen = !Main.IsFullscreen
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
		_fade.FadeOutTime = 1f;
		_fade.ForceFadeOut();
	}

	public override void Update()
	{
		_fade.Update();
		_background.Size = Main.WindowSize;
		if (CurrentGroup == _settings && Input.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
		{
			_fade.OnFaded = () => CurrentGroup = _mainMenu;
			_fade.StartFading();
		}

		base.Update();
	}

	public override void Draw()
	{
		_fade.Draw();
		base.Draw();
	}
}
