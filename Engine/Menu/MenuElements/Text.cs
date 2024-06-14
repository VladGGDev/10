using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

namespace Menus;

public class Text : MenuElement
{
	SpriteFont _font;
	public string String { get; set; } = "text";

	public Vector2 Padding { get; set; } = Vector2.Zero;

	public Color NormalColor { get; set; } = Color.Black;
	public Color SelectedColor { get; set; } = Color.White;
	FloatTween _colorTween;

	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light");

		_colorTween = new FloatTween(0.15f);
		OnSelected = () => _colorTween.SetStart(0).SetTarget(1).RestartAt(1 - _colorTween.EasedElapsedPercentage);
		OnDeselected = () => _colorTween.SetStart(1).SetTarget(0).RestartAt(1 - _colorTween.EasedElapsedPercentage);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = _font.MeasureString(String);

		spriteBatch.DrawString(
			_font,
			String,
			Position,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			textSize * Pivot,
			1f * Size,
			SpriteEffects.None,
			0.91f);

		spriteBatch.Draw(
			Main.Pixel,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, _colorTween.Result()),
			0,
			Pivot,
			textSize * Size + Padding,
			SpriteEffects.None,
			0.9f);
	}
}