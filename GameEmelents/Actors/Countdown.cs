using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

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

	public Countdown(float time)
	{
		StartTime = Time = time; // I didn't know you could do this in C#
	}


	public override void Start(ContentManager content)
	{
		_font = content.Load<SpriteFont>("Fonts/Roboto-Light");
		_player = SceneManager.CurrentScene.GetActor<Player>();
	}

	public override void Update()
	{
		if (!_started)
		{
			if (Input.GetKeyDown(Keys.A) || Input.GetKeyDown(Keys.D) || Input.GetKeyDown(Keys.W) ||
				Input.GetKeyDown(Keys.Left) || Input.GetKeyDown(Keys.Right) || Input.GetKeyDown(Keys.Up) ||
				Input.GetKeyDown(Keys.Space))
				_started = true;
			else
				return;
		}
		Time -= Main.DeltaTime;
		if (Time <= 0 && !_lost)
			Lose();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		string text = MathF.Round(MathF.Max(0, Time)).ToString();
		spriteBatch.DrawString(_font,
			text,
			Main.WindowCenter,
			Color.WhiteSmoke,
			0f,
			_font.MeasureString(text) / 2,
			1.5f,
			SpriteEffects.None,
			0.0f);
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
			_ = new Timeout(250f, () =>
			{
				SceneManager.ChangeScene(SceneManager.CurrentSceneIndex);
			});
		});
	}

	public void NextLevel()
	{
		_won = true;
		CanPlayerMove = false;
		CanPlayerSimulate = false;
		_ = new Timeout(1000f, () =>
		{
			SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1);
		});
	}
}
