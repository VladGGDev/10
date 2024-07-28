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

	public static Main Instance { get; private set; }

	public static bool CanDraw = true;

	// 1x1 pixel texture
	public static Texture2D Pixel;

	// Debug message
	public static string DebugMessage = "";
	SpriteFont _font;

	// Screen data
	Point _renderTargetPos;
	RenderTarget2D _renderTarget;
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
	public const float TargetScreenHeight = 1080;
	public const float TargetAspectRatio = 16f / 9f;
	public static readonly Vector2 EntityLayerOffset = new(8, 8);  // Don't forget about this
	public static bool DebugGraphics = true;


	public Main()
	{
		Instance = this;
		_graphicsSettings = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
	}

	protected override void Initialize()
	{
		_spriteDraw = new SpriteBatch(GraphicsDevice);
		_sceneManager = new(Content, _spriteDraw);
		//_renderTarget = new(GraphicsDevice, (int)(TargetScreenHeight * TargetAspectRatio), (int)TargetScreenHeight);
		Window.ClientSizeChanged += HandleWindowSizeChange;

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
			_sceneManager.AddScene<GameScene>(level, 10f);
			// TODO: In between each scene add a scene for the number of the level
		}

		SceneManager.ChangeScene(0);
		_sceneManager._HandleQueuedScene();


		base.Initialize();
	}

	protected override void BeginRun()
	{
		HandleWindowSizeChange(null, null);
		base.BeginRun();
	}

	protected override void LoadContent()
	{
		_font = Content.Load<SpriteFont>("Fonts/Roboto-Light");
	}
	
	protected override void Update(GameTime gameTime)
	{
		if (!IsActive)
			return;

		if (Input.GetKeyDown(Keys.Escape)) // Delete when game is finished
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
		if (Input.GetMouseButtonDown(0))
			SceneManager.ChangeScene(SceneManager.CurrentSceneIndex - 1);
		if (Input.GetMouseButtonDown(1))
			SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1);
			//SceneManager.ChangeScene(1);

		if (Input.GetKeyDown(Keys.Q))
			CanDraw = !CanDraw;

		_sceneManager._HandleQueuedScene();

		// Input (has to happen last)
		Input._UpdatePrevInput();

		base.Update(gameTime);
	}

	protected override bool BeginDraw()
	{
		return CanDraw;
	}

	protected override void Draw(GameTime gameTime)
	{
		if (!IsActive)
			return;

		// Camera operations
		Vector2 clientBounds = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
		float camScale = TargetScreenHeight / Camera.Instance.Size;
		float windowScale = MathF.Min(clientBounds.Y, clientBounds.X / TargetAspectRatio) / TargetScreenHeight;

		float cameraSize = camScale * windowScale;
		Vector2 cameraPosition = -Camera.Instance.Position; // Follow camera (top left corner)
		cameraPosition += (clientBounds * (camScale / windowScale)) / 2f; // Center camera

		Vector2 blackBarOffset = new Vector2(clientBounds.X - _renderTarget.Width, clientBounds.Y - _renderTarget.Height) / 2f;
		cameraPosition -= blackBarOffset / windowScale;



		// Drawing to target
		GraphicsDevice.SetRenderTarget(_renderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteDraw.Begin(
			samplerState: SamplerState.PointClamp,
			sortMode: SpriteSortMode.FrontToBack,
			blendState: BlendState.NonPremultiplied,
			transformMatrix:
				Matrix.CreateTranslation(new(cameraPosition, 0)) *
				Matrix.CreateScale(cameraSize));

		// UI screen data
		WindowSize = Camera.Instance.Dimensions;
		WindowCenter = -cameraPosition + WindowSize / 2f;
		//_spriteDraw.DrawSimlple(Pixel, WindowCenter - (WindowSize / 2f), 0, WindowSize, Color.Gold, 1f); // Fullscreen panel test

		// Drawing the current scene
		SceneManager.CurrentScene.Draw(_sceneManager.SpriteBatch, _sceneManager.LevelRenderer);

		// Debug text
		string text = DebugMessage;
		Vector2 textMiddlePoint = _font.MeasureString(text) / 2f;
		_spriteDraw.DrawString(_font, text, WindowCenter, Color.Black, 0f, textMiddlePoint, 1f, SpriteEffects.None, 1f);

		_spriteDraw.End();



		// Drawing target to screen
		GraphicsDevice.SetRenderTarget(null);
		_spriteDraw.Begin();
		_spriteDraw.Draw(
			_renderTarget,
			new Rectangle(_renderTargetPos.X, _renderTargetPos.Y, _renderTarget.Width, _renderTarget.Height),
			Color.White);
		_spriteDraw.End();

		base.Draw(gameTime);
	}



	void HandleWindowSizeChange(object o, EventArgs e)
	{
		Point newScreenSize = Window.ClientBounds.Size;
		_renderTargetPos = new(0);
		if (newScreenSize.X < newScreenSize.Y * TargetAspectRatio)
		{
			// Horizontal black bars
			newScreenSize.Y = (int)(newScreenSize.X / TargetAspectRatio);
			_renderTargetPos.Y = (Window.ClientBounds.Height - newScreenSize.Y) / 2;
		}
		else
		{
			// Vecrtical black bars
			newScreenSize.X = (int)(newScreenSize.Y * TargetAspectRatio);
			_renderTargetPos.X = (Window.ClientBounds.Width - newScreenSize.X) / 2;
		}
		_renderTarget = new(GraphicsDevice, newScreenSize.X, newScreenSize.Y);
	}
}
