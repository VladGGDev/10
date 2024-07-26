using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Menus;
using Tweening;
using Microsoft.Xna.Framework.Content;
using System.Runtime.CompilerServices;

public class Fade : MenuElement
{
	public Color Color { get; set; } = Color.Black;

	public Action OnFaded { get; set; }
	public float FadeInTime { get; set; } = 0.25f;
	public float FadeStayTime { get; set; } = 0.1f;
	public float FadeOutTime { get; set; } = 0.25f;

	FloatTween _tween = new();

	float _alpha = 0;
	FadeState _fadeState = FadeState.Idle;

	enum FadeState
	{
		Idle,
		FadingIn,
		Faded,
		FadingOut
	}

	public override void Start(ContentManager content)
	{
		OnSelected = () => throw new Exception("Fades cannot be selectable.");
	}

	public override void Update()
	{
		switch (_fadeState)
		{
			case FadeState.Idle:
				_alpha = 0;
				break;
			case FadeState.FadingIn:
				_alpha = _tween.Result();
				if (!_tween.IsRunning)
				{
					_tween.SetStart(1).SetTarget(1).SetDuration(FadeStayTime).Restart();
					_fadeState = FadeState.Faded;
					OnFaded?.Invoke();
				}
				break;
			case FadeState.Faded:
				_alpha = 1;
				if (!_tween.IsRunning)
				{
					_tween.SetStart(1).SetTarget(0).SetDuration(FadeOutTime).Restart();
					_fadeState = FadeState.FadingOut;
				}
				break;
			case FadeState.FadingOut:
				_alpha = _tween.Result();
				if (!_tween.IsRunning)
				{
					_fadeState = FadeState.Idle;
				}
				break;
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Color col = new Color(Color, _alpha);
		spriteBatch.Draw(
			Main.Pixel,
			Main.WindowCenter,
			null,
			col,
			0,
			Vector2.One / 2f,
			Main.WindowSize,
			SpriteEffects.None,
			1f);
	}



	public void StartFading()
	{
		if (_fadeState != FadeState.Idle)
			return;
		_fadeState = FadeState.FadingIn;
		_tween.SetStart(0).SetTarget(1).SetDuration(FadeInTime).Restart();
	}
}