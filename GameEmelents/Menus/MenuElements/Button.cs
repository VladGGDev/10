using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tweening;
using System;

namespace Menus.MenuElements;

public class Button : MenuElement
{
	public Texture2D Texture { get; set; }

	public Vector2 Padding { get; set; } = Vector2.Zero;

	public Color NormalColor { get; set; } = Color.Black;
	public Color SelectedColor { get; set; } = Color.White;
	FloatTween _colorTween;

	public Action OnInteract { get; set; }

	public bool UseUnscaledTime = false;

	public override void Start(ContentManager content)
	{
		_colorTween = new FloatTween(0.15f) { UseUnscaledTime = UseUnscaledTime };
		OnSelected = () => _colorTween.SetStart(0).SetTarget(1).RestartAt(1 - _colorTween.EasedElapsedPercentage);
		OnDeselected = () => _colorTween.SetStart(1).SetTarget(0).RestartAt(1 - _colorTween.EasedElapsedPercentage);
	}

	public override void Update()
	{
		if (IsSelected && Input.GetActionDown("MenuInteract") && OnInteract != null)
			OnInteract();
	}

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes["UI"];
		pass.Draw(
			Texture,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, 1 - _colorTween.Result()),
			0,
			Texture.Bounds.Size.ToVector2() * Pivot,
			Size, 
			SpriteEffects.None,
			LayerDepth);

		pass.Draw(
			Main.Pixel,
			Position,
			null,
			Color.Lerp(NormalColor, SelectedColor, _colorTween.Result()),
			0,
			Pivot,
			Texture.Bounds.Size.ToVector2() * Size + Padding,
			SpriteEffects.None,
			LayerDepth - 0.01f);
	}
}
