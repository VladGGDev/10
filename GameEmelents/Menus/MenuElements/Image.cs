using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

namespace Menus.MenuElements;

public class Image : MenuElement
{
	public Texture2D Texture { get; set; }

	public Vector2 Padding { get; set; } = Vector2.Zero;

	public Color Color { get; set; } = Color.White;
	public Color BackgroundColor { get; set; } = Color.Transparent;

	public override void Start(ContentManager content)
	{
		OnSelected = () => throw new Exception("Images cannot be selectable.");
	}

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes["UI"];
		pass.Draw(
			Texture,
			Position,
			null,
			Color,
			0,
			Texture.Bounds.Size.ToVector2() * Pivot,
			Size,
			SpriteEffects.None,
			LayerDepth);

		pass.Draw(
			Main.Pixel,
			Position,
			null,
			BackgroundColor,
			0,
			Pivot,
			Texture.Bounds.Size.ToVector2() * Size + Padding,
			SpriteEffects.None,
			LayerDepth - 0.01f);
	}
}