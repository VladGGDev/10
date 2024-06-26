using System;
using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class SceneManager
{
	// LDtk
	LDtkFile _levelFile;
	LDtkWorld _world;

	public LDtkLevel[] WorldLevels => _world.Levels;

	// Renderer
	public ExampleRenderer LevelRenderer { get; private set; }
	public SpriteBatch SpriteBatch { get; private set; }

	// Content
	public ContentManager _content;

	// Scene management
	static int _queuedSceneIndex = -1;
	public static int CurrentSceneIndex { get; private set; }
	public static Scene CurrentScene { get; private set; }

	struct SceneData
	{
		public Type SceneType;
		public object[] ConstructorParams;
	}
	List<SceneData> _sceneData;

	public SceneManager(ContentManager content, SpriteBatch spriteBatch)
	{
		_sceneData = new();
		_content = content;
		SpriteBatch = spriteBatch;
		LevelRenderer = new ExampleRenderer(spriteBatch);

		_levelFile = LDtkFile.FromFile("Content/10 level.ldtk");
		_world = _levelFile.LoadWorld(LDtkTypes.Worlds.World.Iid);

		foreach (var level in _world.Levels)
		{
			LevelRenderer.PrerenderLevel(level);
		}
	}

	public void AddScene<SceneT>(params object[] constructorParams) where SceneT : Scene
	{
		_sceneData.Add(new() { SceneType = typeof(SceneT), ConstructorParams = constructorParams });
	}


	public static void ChangeScene(int sceneIndex)
	{
		_queuedSceneIndex = sceneIndex;
	}

	internal void _HandleQueuedScene()
	{
		if (_queuedSceneIndex == -1)
			return;
		CurrentScene?.End();
		CurrentSceneIndex = _queuedSceneIndex;
		CurrentScene = Activator.CreateInstance(_sceneData[_queuedSceneIndex].SceneType, _sceneData[_queuedSceneIndex].ConstructorParams) as Scene;
		CurrentScene.Start(_content);
		_queuedSceneIndex = -1;
	}
}
