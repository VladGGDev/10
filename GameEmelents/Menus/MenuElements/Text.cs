using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

namespace Menus.MenuElements;

public class Text : MenuElement
{
	public SpriteFont Font { get; set; }
	public string String { get; set; } = "text";

	public Vector2 Padding { get; set; } = Vector2.Zero;

	public Color Color { get; set; } = Color.White;
	public Color BackgroundColor { get; set; } = Color.Transparent;

	public override void Start(ContentManager content)
	{
		OnSelected = () => throw new Exception("Text cannot be selectable.");
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = Font.MeasureString(String);
		spriteBatch.DrawString(
			Font,
			String,
			Position,
			Color,
			0,
			textSize / 2f,
			1f * Size,
			SpriteEffects.None,
			LayerDepth);

		spriteBatch.Draw(
			Main.Pixel,
			Position,
			null,
			BackgroundColor,
			0,
			Pivot,
			textSize * Size + Padding,
			SpriteEffects.None,
			LayerDepth - 0.1f);
	}
}