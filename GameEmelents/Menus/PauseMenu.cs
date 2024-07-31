using System;
using Menus;
using Menus.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tweening;

public class PauseMenu : Menu
{
	Image _background;
	Button _playButton;
	Button _exitButton;
	Button _retryButton;

	bool _isPaused = false;
	public bool IsPaused
	{
		get => _isPaused;
		set
		{
			_isPaused = value;
			if (value)
			{
				_alphaTween.SetStart(0).SetTarget(1f).Restart();
				Main.TimeScale = 0;
			}
			else
			{
				_alphaTween.SetStart(1f).SetTarget(0).Restart();
				_ = new Timeout(1, () => Main.TimeScale = 1);
			}
		}
	}
	FloatTween _alphaTween = new(0.15f) { UseUnscaledTime = true };

	const float backgroundAlpha = 0.5f;
	const float buttonDistance = 200f;
	const float buttonSize = 0.25f;

	public override void Start(ContentManager content)
	{
		Main.TimeScale = 1;

		_background = new()
		{
			Texture = Main.Pixel,
			Color = new(Color.Black, 0.5f),
			LayerDepth = 0.97f
		};

		Fade fade = new()
		{
			FadeInTime = 0.5f,
			FadeStayTime = 0f,
		};

		_playButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Play"),
			Size = new(buttonSize),
			LayerDepth = 0.99f,
			UseUnscaledTime = true,

			OnInteract = () => IsPaused = false
		};

		_exitButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Exit"),
			Offset = new(-buttonDistance, 0),
			Size = new(buttonSize),
			LayerDepth = 0.99f,
			UseUnscaledTime = true,

			OnInteract = () =>
			{
				fade.OnFaded = () =>
				{
					Main.TimeScale = 1f;
					SceneManager.ChangeScene(0);
				};
				fade.StartFading();
			}
		};

		_retryButton = new()
		{
			Texture = content.Load<Texture2D>("Icons/Retry"),
			Offset = new(buttonDistance, 0),
			Size = new(buttonSize),
			LayerDepth = 0.99f,
			UseUnscaledTime = true,

			OnInteract = () =>
			{
				IsPaused = false;
				SceneManager.CurrentScene.GetActor<Countdown>().Lose();
			}
		};


		CurrentGroup = new(_playButton);
		CurrentGroup.AddNode(_exitButton);
		CurrentGroup.AddLeftLink(_playButton, _exitButton);
		CurrentGroup.AddNode(_retryButton);
		CurrentGroup.AddRightLink(_playButton, _retryButton);
		CurrentGroup.AddNode(_background);
		CurrentGroup.AddNode(fade);

		base.Start(content);
	}

	public override void Update()
	{
		_background.Size = Main.WindowSize;
		_background.Color = new(_background.Color, _alphaTween.Result() * backgroundAlpha);
		_playButton.NormalColor = new(_playButton.NormalColor, _alphaTween.Result());
		_playButton.SelectedColor = new(_playButton.SelectedColor, _alphaTween.Result());
		_exitButton.NormalColor = new(_exitButton.NormalColor, _alphaTween.Result());
		_exitButton.SelectedColor = new(_exitButton.SelectedColor, _alphaTween.Result());
		_retryButton.NormalColor = new(_retryButton.NormalColor, _alphaTween.Result());
		_retryButton.SelectedColor = new(_retryButton.SelectedColor, _alphaTween.Result());

		if (Input.GetKeyDown(Keys.Escape))
			IsPaused = !IsPaused;

		if (IsPaused)
			base.Update();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
	}
}
