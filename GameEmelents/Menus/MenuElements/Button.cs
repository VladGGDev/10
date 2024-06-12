using Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

public class Button : MenuElement
{
	SpriteFont _font;
	public string Text;

	public Vector2 Padding;

	public Color NormalColor;
	public Color SelectedColor;
	FloatTween _colorTween;

	public Action OnInteract;

	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light");

		_colorTween = new FloatTween(0.15f);
		OnSelected = () => _colorTween.SetStart(0).SetTarget(1).Restart();
		OnDeselected = () => _colorTween.SetStart(1).SetTarget(0).Restart();
	}

	public override void Update()
	{
		if (IsSelected && Input.GetActionDown("MenuInteract") && OnInteract != null)
			OnInteract();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = _font.MeasureString(Text);

		spriteBatch.DrawString(
			_font, 
			Text,
			Position, 
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			textSize * Pivot,
			1f, 
			SpriteEffects.None,
			0.91f);

		spriteBatch.Draw(
			Main.Pixel,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, _colorTween.Result()),
			0,
			Pivot,
			textSize + Padding,
			SpriteEffects.None,
			0.1f);
	}
}
