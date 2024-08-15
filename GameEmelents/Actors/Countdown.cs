using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Menus.MenuElements;
using System.Diagnostics;
using Tweening;

public class Countdown : Actor
{
	public bool CanPlayerMove { get; set; } = true;
	public bool CanPlayerSimulate { get; set; } = true;

	SpriteFont _font;
	Player _player;

	public float StartTime { get; set; }
	public float Time { get; set; }
	
	// Juice
	int _nextSecond;
	const float _maxAlpha = 0.5f;
	const float _tweenScaleMult = 0.025f;
	FloatTween _tweenScale = new() { UseUnscaledTime = true };
	FloatTween _tweenAlpha = new() { UseUnscaledTime = true };
	bool _lostBcTimer = false;

	bool _started = false;
	bool _won = false;
	bool _lost = false;

	Fade _fade = new() 
	{ 
		FadeInTime = 0.5f,
		FadeOutTime = 0.5f
	};

	public Countdown(float time)
	{
		StartTime = Time = time; // I didn't know you could do this in C#
	}


	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light-Big");
		_player = SceneManager.CurrentScene.GetActor<Player>();


		SceneManager.CurrentScene.FirstUpdate += (o, e) => FadeFromBlack();

		_nextSecond = (int)Time;
		_tweenScale.SetStart(0).SetTarget(1f).SetDuration(0.5f).SetEasing(EasingFunctions.YoYo(3));
		_tweenAlpha.SetStart(0).SetTarget(_maxAlpha).SetDuration(0.5f).SetEasing(EasingFunctions.YoYo(3));
		_tweenScale.RestartAt(1f);
		_tweenAlpha.RestartAt(1f);
	}

	public override void Update()
	{
		_fade.Update();
		if (!_started && Main.TimeScale != 0)
		{
			if (Input.GetActionDown("Left") || Input.GetActionDown("Right") || Input.GetActionDown("Jump"))
				_started = true;
			else
				return;
		}

		if (!_won && !_lost)
			Time -= Main.DeltaTime;

		if (Time < _nextSecond)
		{
			_nextSecond--;
			_tweenScale.Restart();
			_tweenAlpha.Restart();
		}

		if (Time <= 0 && !_lost)
		{
			_lostBcTimer = true;
			_tweenAlpha.SetDuration(_tweenAlpha.Duration * 2f).SetEasing(EasingFunctions.YoYo(5));
			_tweenScale.SetDuration(_tweenScale.Duration * 2f).SetEasing(EasingFunctions.YoYo(5));
			Lose();
		}
	}

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes["UI"];
		string text = MathF.Floor(MathF.Max(0, Time + 1)).ToString();
		pass.DrawString(_font,
			text,
			Main.WindowSize / 2f,
			new(Color.WhiteSmoke, _tweenAlpha.Result()),
			0,
			_font.MeasureString(text) / 2 - new Vector2(64f, 0),
			1f + _tweenScale.Result() * _tweenScaleMult,
			SpriteEffects.None,
			0.8f);

		_fade.Draw();
	}


	public void Lose()
	{
		if (_won)
			return;
		_lost = true;
		Main.TimeScale = 0;
		CanPlayerSimulate = false;
		_ = new Timeout(_lostBcTimer ? 1000f : 250f, () =>
		{
			SceneManager.CurrentScene.RemoveActor(_player);
			_fade.OnFaded = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex);
			FadeToBlack();
			//_ = new Timeout(250f, () =>
			//{
			//	SceneManager.ChangeScene(SceneManager.CurrentSceneIndex);
			//});
		});
	}

	public void NextLevel()
	{
		_won = true;
		Main.TimeScale = 0;
		CanPlayerMove = false;
		CanPlayerSimulate = false;
		_ = new Timeout(1000f - _fade.FadeInTime * 1000, () =>
		{
			_fade.OnFaded = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1);
			FadeToBlack();
		});
	}


	void FadeToBlack()
	{
		_fade.StartFading();
	}

	void FadeFromBlack()
	{
		_fade.ForceFadeOut();
	}
}
