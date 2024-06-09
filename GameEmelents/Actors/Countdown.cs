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

	float _timer = 10f;
	bool _started = false;
	bool _won = false;
	bool _lost = false;

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
		_timer -= Main.DeltaTime;
		if (_timer <= 0 && !_lost)
			Lose();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		string text = Math.Clamp(_timer, 0, 10f).ToString("#0");
		spriteBatch.DrawString(_font,
			text,
			new(500, 500), Color.WhiteSmoke,
			0f,
			_font.MeasureString(text) / 2,
			1f,
			SpriteEffects.None,
			0.3f);
	}


	public void Lose()
	{
		if (_won)
			return;
		_lost = true;
		CanPlayerSimulate = false;
		new Timeout(250f, () =>
		{
			SceneManager.CurrentScene.RemoveActor(_player);
			new Timeout(500f, () =>
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
		new Timeout(1000f, () =>
		{
			SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1);
		});
	}
}
