using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;

public class GameScene : Scene
{
	List<BoxCollider> _levelColliders = new List<BoxCollider>();
	

	public GameScene(LDtkLevel level, float time) : base(level)
	{
		Player player = Level.GetEntity<Player>();
		Camera = new(player.Position, Main.TargetScreenHeight * 1f, new(Level.Position, Level.Size - new Point(0, 8)));
		
		AddActorRange(Level.GetEntities<Coin>());
		AddActorRange(Level.GetEntities<Saw>());
		AddActor(player);
		AddActor(new Countdown(time));
	}


	public override void Start(ContentManager content)
	{
		base.Start(content);

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
			new Vector2(Main.UnitSize,  levelSize.Y),
			"Level")); // Left
		_levelColliders.Add(new(
			levelPos + new Vector2(levelSize.X + Main.UnitSize / 2f, levelSize.Y / 2f),
			new Vector2(Main.UnitSize, levelSize.Y),
			"Level")); // Right
		_levelColliders.Add(new(
			levelPos + new Vector2(levelSize.X / 2f, -Main.UnitSize / 2f),
			new Vector2(levelSize.X, Main.UnitSize),
			"Level")); // Up
		_levelColliders.Add(new(
			levelPos + new Vector2(levelSize.X / 2f, levelSize.Y + Main.UnitSize / 2f),
			new Vector2(levelSize.X, Main.UnitSize),
			"Spike")); // Down
	}

	public override void Draw(SpriteBatch spriteBatch, ExampleRenderer levelRenderer)
	{
		base.Draw(spriteBatch, levelRenderer);

		// Debug draw the colliders
		foreach (var col in _levelColliders)
		{
			Color c = col.Tag switch
			{
				"Level" => Color.Blue,
				"Spike" => Color.Red,
				_ => Color.Pink,
			};
			//c.A = 64;
			//spriteBatch.Draw(Main.Pixel, col.Position, null, c, 0, Vector2.One / 2f, col.Size, SpriteEffects.None, 0.05f);
			spriteBatch.DrawSquareOutline(col.Position, col.Size, c);
		}
	}
}
