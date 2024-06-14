using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

namespace Menus.MenuElements;

public class Slider : MenuElement
{
	SpriteFont _font;
	public string Text { get; set; } = "";

	public Vector2 Padding { get; set; } = Vector2.Zero;
	public Vector2 SliderSize { get; set; } = new(300f, 15f);
	public Vector2 SliderHandleSize { get; set; } = new(10f, 45f);
	public float Gap { get; set; } = 25f;

	public Color NormalColor { get; set; } = Color.Black;
	public Color SelectedColor { get; set; } = Color.White;
	FloatTween _colorTween;

	public Action<float> OnValueChanged { get; set; }
	public float Value { get; set; } = 0;
	public float Min { get; set; } = 0;
	public float Max { get; set; } = 1f;
	public float Step { get; set; } = 0.1f;
	public int Decimals { get; set; } = 1;

	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light");

		_colorTween = new FloatTween(0.15f);
		OnSelected = () => _colorTween.SetStart(0).SetTarget(1).Restart();
		OnDeselected = () => _colorTween.SetStart(1).SetTarget(0).Restart();
	}

	public override void Update()
	{
		if (IsSelected)
		{
			if (Input.GetActionDown("MenuLeft"))
			{
				Value = Math.Clamp(Value - Step, Min, Max);
				OnValueChanged?.Invoke(Value);
			}
			if (Input.GetActionDown("MenuRight"))
			{
				Value = Math.Clamp(Value + Step, Min, Max);
				OnValueChanged?.Invoke(Value);
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = _font.MeasureString(Text);
		string valueText = MathF.Round(Value, Decimals).ToString($"F{Decimals}");
		Vector2 valueTextSize = _font.MeasureString(valueText);

		// Text
		Vector2 textPosition = Position - Gap * Vector2.UnitX - SliderSize.X * Vector2.UnitX / 2f - Vector2.UnitX * textSize.X / 2f;
		spriteBatch.DrawString(
			_font,
			Text,
			textPosition,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			textSize * Pivot,
			1f,
			SpriteEffects.None,
			0.91f);

		// Slider
		spriteBatch.Draw(
			Main.Pixel,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			Pivot,
			SliderSize,
			SpriteEffects.None,
			0.91f);

		// Slider handle
		spriteBatch.Draw(
			Main.Pixel,
			new Vector2(LerpExtensions.Remap(Min, Max, Position.X - SliderSize.X / 2, Position.X + SliderSize.X / 2, Value), Position.Y),
			null,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			Pivot,
			SliderHandleSize,
			SpriteEffects.None,
			0.91f);

		// Value
		Vector2 valueTextPosition = Position + Gap * Vector2.UnitX + SliderSize.X * Vector2.UnitX / 2f + Vector2.UnitX * valueTextSize.X / 2f;
		spriteBatch.DrawString(
			_font,
			valueText,
			valueTextPosition,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			valueTextSize * Pivot,
			1f,
			SpriteEffects.None,
			0.91f);

		// Background
		spriteBatch.Draw(
			Main.Pixel,
			(textPosition - Vector2.UnitX * textSize.X / 2f + valueTextPosition + Vector2.UnitX * valueTextSize.X / 2f) / 2f,
			null,
			Color.Lerp(NormalColor, SelectedColor, _colorTween.Result()),
			0,
			Pivot,
			textSize + Padding + Vector2.UnitX * Gap * 2f + Vector2.UnitX * SliderSize.X + Vector2.UnitX * valueTextSize.X,
			SpriteEffects.None,
			0.9f);
	}
}
