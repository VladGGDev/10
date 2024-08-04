using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Menus.MenuElements;
using System.Diagnostics;

public class Countdown : Actor
{
	public bool CanPlayerMove { get; set; } = true;
	public bool CanPlayerSimulate { get; set; } = true;

	SpriteFont _font;
	Player _player;

	public float StartTime { get; set; }
	public float Time { get; set; }
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
	}

	public override void Update()
	{
		_fade.Update();
		if (!_started)
		{
			if (Input.GetActionDown("Left") || Input.GetActionDown("Right") || Input.GetActionDown("Jump"))
				_started = true;
			else
				return;
		}
		if (!_won && !_lost)
			Time -= Main.DeltaTime;
		if (Time <= 0 && !_lost)
			Lose();
	}

	public override void Draw()
	{
		DrawPass pass = DrawPass.Passes["UI"];
		string text = MathF.Round(MathF.Max(0, Time)).ToString();
		pass.DrawString(_font,
			text,
			Main.WindowSize / 2f,
			Color.WhiteSmoke,
			0f,
			_font.MeasureString(text) / 2,
			1f,
			SpriteEffects.None,
			0.8f);

		_fade.Draw();
	}


	public void Lose()
	{
		if (_won)
			return;
		_lost = true;
		CanPlayerSimulate = false;
		_ = new Timeout(250f, () =>
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
