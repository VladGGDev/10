using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;

public class GameScene : Scene
{
	List<BoxCollider> _levelColliders = new List<BoxCollider>();
	PauseMenu _pauseMenu = new();

	public GameScene(LDtkLevel level, float time, Effect backgroundEffect) : base(level)
	{
		DrawPass.Passes["Background"].Settings.Effect = backgroundEffect;

		Player player = Level.GetEntity<Player>();
		Camera = new(player.Position, Main.TargetScreenHeight * 1f, new(Level.Position, Level.Size - new Point(0, 8)));
		
		AddActorRange(Level.GetEntities<Coin>()); // Or Actors.AddRange(Level.GetEntities<Coin>());
		AddActorRange(Level.GetEntities<Saw>());
		AddActor(player); // Or Actors.Add(player);
		AddActor(new Countdown(time));
	}


	public override void Start(ContentManager content)
	{
		base.Start(content);

		_pauseMenu.Start(content);

		// Level collider
		LDtkIntGrid intGrid = Level.GetIntGrid("Level");
		for (int i = 0; i < intGrid.Values.Length; i++)
		{
			// Skip over air
			if (intGrid.Values[i] == 0)
				continue;

			// Get 2D position from 1D
			Vector2 pos = new(i % intGrid.GridSize.X, i / intGrid.GridSize.X);
			pos *= intGrid.TileSize;  // Correct size
			pos += Vector2.One * intGrid.TileSize / 2f;  // Correct pivot

			// Change collider based on the type of tile
			(float size, string tag) = intGrid.Values[i] switch
			{
				1 => (intGrid.TileSize, "Level"),
				2 => (intGrid.TileSize * 0.1f, "Spike"),
				_ => (0, "Error"),
			};
			_levelColliders.Add(new BoxCollider(pos - Vector2.One, Vector2.One * size, tag));
		}

		// Level bounds collider
		Vector2 levelPos = Level.Position.ToVector2();
		Vector2 levelSize = Level.Size.ToVector2();
		_levelColliders.Add(new(
			levelPos + new Vector2(-Main.UnitSize / 2f, levelSize.Y / 2f),
			new Vector2(Main.UnitSize, levelSize.Y + Main.UnitSize * 20f),
			"Level")); // Left
		_levelColliders.Add(new(
			levelPos + new Vector2(levelSize.X + Main.UnitSize / 2f, levelSize.Y / 2f),
			new Vector2(Main.UnitSize, levelSize.Y + Main.UnitSize * 20f),
			"Level")); // Right
		//_levelColliders.Add(new(
		//	levelPos + new Vector2(levelSize.X / 2f, -Main.UnitSize / 2f),
		//	new Vector2(levelSize.X, Main.UnitSize),
		//	"Level")); // Up
		_levelColliders.Add(new(
			levelPos + new Vector2(levelSize.X / 2f, levelSize.Y + Main.UnitSize / 2f + Main.UnitSize * 4),
			new Vector2(levelSize.X + Main.UnitSize * 10f, Main.UnitSize),
			"Spike")); // Down
	}

	public override void Update()
	{
		_pauseMenu.Update();
		base.Update();
	}

	public override void Draw(ExampleRenderer levelRenderer)
	{
		_pauseMenu.Draw();
		base.Draw(levelRenderer);

		// Debug draw the colliders
		if (!Main.DebugGraphics)
			return;
		DrawPass pass = DrawPass.Passes[""];
		foreach (var col in _levelColliders)
		{
			Color c = col.Tag switch
			{
				"Level" => Color.Blue,
				"Spike" => Color.Red,
				_ => Color.Pink,
			};
			//c.A = 64;
			pass.DrawSquareOutline(col.Position, col.Size, c);
		}
	}
}
