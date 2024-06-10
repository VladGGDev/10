using Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;

public class Button : MenuElement
{
	SpriteFont _font;
	public string Text;

	public Vector2 Padding;

	public Color NormalColor;
	public Color SelectedColor;
	FloatTween _colorTween;
	bool _wasSelected = false;

	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light");

		_colorTween = new FloatTween(0.15f);
	}

	public override void Update()
	{
		if (!_wasSelected && IsSelected)
			_colorTween.SetStart(0).SetTarget(1).Restart();
		if (_wasSelected && !IsSelected)
			_colorTween.SetStart(1).SetTarget(0).Restart();

		_wasSelected = IsSelected;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = _font.MeasureString(Text);
		Vector2 textMiddle = textSize / 2f;

		spriteBatch.DrawString(
			_font, 
			Text,
			Position, 
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			textMiddle,
			1f, 
			SpriteEffects.None,
			0.91f);

		spriteBatch.Draw(
			Main.Pixel,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, _colorTween.Result()),
			0,
			Vector2.One / 2f,
			textSize + Padding,
			SpriteEffects.None,
			0.1f);
	}
}
