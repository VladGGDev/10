using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using LDtk;
using LDtk.Renderer;
using Tweening;
using System.Diagnostics;

public class Main : Game
{
	GraphicsDeviceManager _graphicsSettings;
	SpriteBatch _spriteDraw;
	SceneManager _sceneManager;

	// 1x1 pixel texture
	public static Texture2D Pixel;

	// Debug message
	public static string DebugMessage = "";
	SpriteFont _font;

	// UI screen data
	public static Vector2 WindowCenter { get; private set; }
	public static Vector2 WindowSize { get; private set; }

	// Time
	public static float TimeScale { get; private set; } = 1f;

	public static float DeltaTime { get; private set; }
	public static float UnscaledDeltaTime { get; private set; }
	public static float UnscaledFixedDeltaTime { get; private set; } = 1f / 144f;
	public static float FixedDeltaTime { get; private set; } = UnscaledFixedDeltaTime * TimeScale;
	public static float TotalTime { get; private set; } = 0;
	public static float UnscaledTotalTime { get; private set; } = 0;

	static float _nextFixedTimestep = 0;


	// Constants
	public const int UnitSize = 32;
	public const float TargetScreenHeight = 1080f;
	public const float TargetAspectRatio = 16f / 9f;


	// LDtk
	//Vector2 _entityLayerOffset = new(0, 8);  // Don't forget about this


	// Testing
	Tween tween = new();

	public Main()
	{
		_graphicsSettings = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";


		tween.SetStart(new(-250, 500))
			.SetTarget(new(500, 500))
			.SetDuration(1f)
			.SetEasing(EasingFunctions.EaseOutBounce);
		//tween.Add(new Tween(new(-250, 500), new(500, 500), 1f))//.SetEasing(EasingFunctions.EaseOutCubic))
		//	.AddDelay(0.5f)
		//	.Add(new Tween(new(-250, 700), new(500, 700), 1f));
	}

	protected override void Initialize()
	{
		_spriteDraw = new SpriteBatch(GraphicsDevice);
		_sceneManager = new(Content, _spriteDraw);

		// Create the 1x1 square texture
		Pixel = new Texture2D(GraphicsDevice, 1, 1);
		Pixel.SetData(new[] { Color.White });

		// Setup
		IsMouseVisible = true;  // Set to false
		IsFixedTimeStep = false;
		//IsFixedTimeStep = true;
		//TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / 144);
		//MaxElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / 60);
		Window.AllowAltF4 = true;
		Window.AllowUserResizing = true;

		// Fullscreen
		//_graphicsSettings.IsFullScreen = true;  // reset to true when pixel perfect works
		_graphicsSettings.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;  // Monitor dimensions
		_graphicsSettings.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
		_graphicsSettings.ApplyChanges();


		// Input actions
		// Player
		Input.CreateAction("Left", Keys.A, Keys.Left);
		Input.CreateAction("Right", Keys.D, Keys.Right);
		Input.CreateAction("Jump", Keys.Space, Keys.W, Keys.Up);
		// Menu
		Input.CreateAction("MenuUp", Keys.W, Keys.Up);
		Input.CreateAction("MenuLeft", Keys.A, Keys.Left);
		Input.CreateAction("MenuDown", Keys.S, Keys.Down);
		Input.CreateAction("MenuRight", Keys.D, Keys.Right);
		Input.CreateAction("MenuInteract", Keys.Space, Keys.Enter);


		// Scenes
		_sceneManager.AddScene<MainMenuScene>(null);
		foreach (var level in _sceneManager.WorldLevels)
		{
			_sceneManager.AddScene<GameScene>(level);
			// In between each scene add a scene for the number of the level
		}
		SceneManager.ChangeScene(0);
		_sceneManager._HandleQueuedScene();



		base.Initialize();
	}

	protected override void LoadContent()
	{
		_font = Content.Load<SpriteFont>("Fonts/Roboto-Light");
	}
	
	protected override void Update(GameTime gameTime)
	{
		if (Input.GetKeyDown(Keys.Escape))
			Exit();

		// Time
		UnscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		UnscaledTotalTime = (float)gameTime.TotalGameTime.TotalSeconds;
		if (UnscaledDeltaTime == 0)
			UnscaledDeltaTime = 0.0000001f;
		DeltaTime = UnscaledDeltaTime * TimeScale;
		TotalTime += DeltaTime;

		// Scene updating
		_nextFixedTimestep += UnscaledDeltaTime;
		while (_nextFixedTimestep > 0)
		{
			_nextFixedTimestep -= UnscaledFixedDeltaTime;
			SceneManager.CurrentScene.FixedUpdate();
		}
		SceneManager.CurrentScene.Update();


		// !!! Test
		if (Input.GetMouseButton(0))
			SceneManager.ChangeScene(0);
		if (Input.GetMouseButton(1))
			SceneManager.ChangeScene(2);
		if (Input.GetKeyDown(Keys.Space))
			tween.Restart();
		if (Input.GetKeyDown(Keys.C))
			tween.RestartAt(0.5f);


		_sceneManager._HandleQueuedScene();
		// Input (has to happen last)
		Input._UpdatePrevInput();

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// Camera operations
		Vector2 clientBounds = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
		float camScale = TargetScreenHeight / Camera.Instance.Size;
		float windowScale = MathF.Min(clientBounds.Y, clientBounds.X / TargetAspectRatio) / TargetScreenHeight;

		Vector2 cameraPosition = -Camera.Instance.Position + (clientBounds * (camScale / windowScale)) / 2f;
		float cameraSize = camScale * windowScale;

		_spriteDraw.Begin(
			samplerState: SamplerState.PointClamp,
			sortMode: SpriteSortMode.FrontToBack,
			blendState: BlendState.NonPremultiplied,
			transformMatrix:
				Matrix.CreateTranslation(new(cameraPosition, 0)) *
				Matrix.CreateScale(cameraSize));

		// UI screen data
		WindowSize = Camera.Instance.Dimensions;
		WindowCenter = -cameraPosition + (clientBounds * (camScale / windowScale)) / 2f;// * cameraSize + WindowSize / 2f;
		//_spriteDraw.Draw(Pixel, new Rectangle(WindowCenter.ToPoint() - (WindowSize / 2f).ToPoint(), WindowSize.ToPoint()), Color.Gold);

		// Drawing the current scene
		SceneManager.CurrentScene.Draw(_sceneManager.SpriteBatch, _sceneManager.LevelRenderer);

		// Debug text
		string text = DebugMessage;
		Vector2 textMiddlePoint = _font.MeasureString(text) / 2f;
		_spriteDraw.DrawString(_font, text, WindowCenter, Color.Black, 0f, textMiddlePoint, 1f, SpriteEffects.None, 1f);

		//// Black bars
		//// Right
		//_spriteDraw.Draw(Pixel, 
		//	new Rectangle(
		//		(int)(WindowCenter.X + WindowSize.X / 2), (int)(WindowCenter.Y - WindowSize.Y / 2),
		//		int.MaxValue, (int)WindowSize.Y + 1),
		//	Color.Black);
		//// Left
		//_spriteDraw.Draw(Pixel,
		//	new Rectangle(
		//		int.MinValue, (int)(WindowCenter.Y - WindowSize.Y / 2),
		//		int.MinValue + (int)(WindowCenter.X - WindowSize.X / 2), (int)WindowSize.Y + 1),
		//	Color.Black);
		//// Up
		//_spriteDraw.Draw(Pixel,
		//	new Rectangle(
		//		(int)(WindowCenter.X - WindowSize.X / 2), int.MinValue,
		//		(int)(WindowSize.X + 1), int.MinValue + (int)(WindowCenter.Y + WindowSize.Y / 2)),
		//	Color.Black);
		//// Down
		//_spriteDraw.Draw(Pixel,
		//	new Rectangle(
		//		(int)(WindowCenter.X - WindowSize.X / 2), (int)(WindowCenter.Y + WindowSize.Y / 2),
		//		(int)(WindowSize.X + 1), int.MaxValue),
		//	Color.Black);


		// Tween test
		_spriteDraw.Draw(Pixel, tween.Result(), null, Color.Beige, 0, Vector2.Zero, 50f, SpriteEffects.None, 1f);

		_spriteDraw.End();

		base.Draw(gameTime);
	}
}
