using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Tweening;
using System.Diagnostics;

internal class Player : Actor
{
	Texture2D _texture;

	// Various
	public Vector2 Velocity = new Vector2();
	Countdown _countdown;

	Vector2 _colliderSizeDiff = new(4, 4);
	bool removedCoinThisFrame = false;

	// Input
	float _input = 0;
	readonly Keys[] _jumpKeys = { Keys.Space, Keys.W, Keys.Up };
	readonly Keys[] _moveLeftKeys = { Keys.A, Keys.Left };
	readonly Keys[] _moveRightKeys = { Keys.D, Keys.Right };

	// Speed
	public float Acceleration = 40f;
	public float Deceleration = 30f;
	public float TurnSpeed = 90f;
	public float MaxSpeed = 3.5f;

	public float AirAcceleration = 25f;
	public float AirDeceleration = 25f;
	public float AirTurnSpeed = 65f;
	public float AirMaxSpeed = 4f;

	bool _isTurning = false;
	public float MaxSpeedToTurn = 2.5f;


	// Jumping and gravity
	public float Gravity = 2000f;
	public float FallingGravity = 2100f;
	public float MaxFallSpeed = 8f;

	public float JumpHeight = Main.UnitSize * 6f;
	public float JumpForce => MathF.Sqrt(2f * Gravity * JumpHeight);
	public float JumpCancelMultiplier = 0.5f;

	public float JumpSpeedBoost = 100f;

	public float BufferTime = 0.1f;
	float _bufferTimer = 0;
	public float CoyoteTime = 0.2f;
	float _coyoteTimer = 0;

	public bool IsGrounded = false;
	bool _wasGrounded = false;


	// Camera
	public float CameraDamping = 0.025f;


	// Juice

	// Turning
	float _rotation = 0;
	public float RotationSpeed = 25f;
	public float RotationAmount = (MathF.PI / 180) * 5f;

	// Squish and stretch
	TweenSequence _usedLandTween;
	TweenSequence _landTween = new();
	TweenSequence _smallLandTween = new();
	public float VelToLand = 2.75f;
	TweenSequence _jumpTween = new();

	Vector2 _sizeMultiplier;

	public override void Start(ContentManager content)
	{
		this.AddCollider(new BoxCollider(Position, Size - _colliderSizeDiff, "Player"));
		//Collider = new BoxCollider(Position, Size - _colliderSizeDif, "Player");

		JumpHeight += _colliderSizeDiff.Y;
		_texture = content.Load<Texture2D>("Square");
		_countdown = SceneManager.CurrentScene.GetActor<Countdown>();

		// Land and jump tweening
		Vector2 landSize = new(1.3f, 0.6f);
		Vector2 smallLandSize = new(1.15f, 0.9f);
		Vector2 jumpSize = new(0.8f, 1.25f);
		_landTween
			.Add(new Tween(Vector2.One, landSize, 0.075f).SetEasing(EasingFunctions.EaseOutBackCustom(3f)))
			.Add(new Tween(landSize, Vector2.One, 0.225f).SetEasing(EasingFunctions.EaseOutBackCustom(3f)));
		_smallLandTween
			.Add(new Tween(Vector2.One, smallLandSize, 0.075f).SetEasing(EasingFunctions.EaseOutBackCustom(3f)))
			.Add(new Tween(smallLandSize, Vector2.One, 0.225f).SetEasing(EasingFunctions.EaseOutBackCustom(3f)));
		_jumpTween
			.Add(new Tween(Vector2.One, jumpSize, 0.1f).SetEasing(EasingFunctions.EaseInOutQuad))
			.Add(new Tween(jumpSize, Vector2.One, 0.25f).SetEasing(EasingFunctions.EaseInOutQuad));
		_landTween.RestartAt(1f);
		_jumpTween.RestartAt(1f);
	}

	public override void Update()
	{
		// Camera
		Camera.Instance.Position = Vector2.Lerp(Camera.Instance.Position, Collider.Position, CameraDamping);

		if (!_countdown.CanPlayerSimulate)
			return;

		// Input
		if (_input > 0 && GetAnyLeftKeyDown())
			_input = -1f;
		if (_input < 0 && GetAnyRightKeyDown())
			_input = 1f;

		if (_input == 0 && GetAnyLeftKey())
			_input = -1;
		if (_input == 0 && GetAnyRightKey())
			_input = 1f;

		if (_input < 0 && GetAnyLeftKeyUp())
			_input = 0;
		if (_input > 0 && GetAnyRightKeyUp())
			_input = 0;


		// Timers
		_bufferTimer -= Main.DeltaTime;
		_coyoteTimer -= Main.DeltaTime;

		// Jumping
		if (GetAnyJumpKeyDown())
			_bufferTimer = BufferTime;

		if (_bufferTimer > 0 && (IsGrounded || _coyoteTimer > 0))
		{
			Velocity.Y = -JumpForce * Main.FixedDeltaTime;
			Velocity.X += _input * JumpSpeedBoost * Main.FixedDeltaTime;
			_coyoteTimer = 0;
			_bufferTimer = 0;
			_jumpTween.Restart();
			if (!GetAnyJumpKey())
				Velocity.Y *= JumpCancelMultiplier;
		}

		if (GetAnyJumpKeyUp() && Velocity.Y < 0)
			Velocity.Y *= JumpCancelMultiplier;


		// Turning
		_rotation = MathHelper.Lerp(_rotation, RotationAmount * MathF.Abs(_input) * MathF.Sign(Velocity.X),
			1 - MathF.Pow(0.5f, Main.DeltaTime * RotationSpeed));

		// Juice
		if (_jumpTween.IsRunning && !IsGrounded)
			_sizeMultiplier = _jumpTween.Result();
		else if (_landTween.IsRunning)
			_sizeMultiplier = _usedLandTween.Result();
		else
			_sizeMultiplier = Vector2.One;
	}

	public override void FixedUpdate()
	{
		if (!_countdown.CanPlayerSimulate)
			return;
		removedCoinThisFrame = false;

		// Gravity and vertical position updating
		Velocity.Y += (Velocity.Y > 0 ? FallingGravity : Gravity) * Main.FixedDeltaTime * Main.FixedDeltaTime;
		if (Velocity.Y > MaxFallSpeed)
			Velocity.Y = MaxFallSpeed;
		Collider.Position += Vector2.UnitY * Velocity.Y;



		// Ground collision checking
		_wasGrounded = IsGrounded;
		IsGrounded = false;
		float prevYVel = Velocity.Y;
		HandleCollision(CollisionAxis.Vertical);



		// Coyote time
		if (_wasGrounded && !IsGrounded && Velocity.Y > 0 && _coyoteTimer <= 0)
			_coyoteTimer = CoyoteTime;

		// Squish land
		if (!_wasGrounded && IsGrounded)
		{
			_usedLandTween = prevYVel > VelToLand ? _landTween : _smallLandTween;
			_landTween.Restart();
			_smallLandTween.Restart();
			//_usedLandTween.Restart();
		}



		// Setting of the speed
		float speedResult;
		(float acc, float dec, float turn, float maxSp) = IsGrounded switch
		{
			true => (Acceleration, Deceleration, TurnSpeed, MaxSpeed),
			false => (AirAcceleration, AirDeceleration, AirTurnSpeed, AirMaxSpeed)
		};
		acc *= Main.FixedDeltaTime;
		dec *= Main.FixedDeltaTime;
		turn *= Main.FixedDeltaTime;

		if (_input != 0)
		{
			if ((MathF.Sign(Velocity.X) == _input || Velocity.X == 0) && !_isTurning)
			{
				// Acceleration
				speedResult = Approach(Velocity.X, maxSp * _input, acc);
			}
			else
			{
				// Turning
				_isTurning = true;
				speedResult = Approach(Velocity.X, maxSp * _input, turn);
			}
		}
		else
		{
			// Deceleration
			speedResult = Approach(Velocity.X, 0, dec);
		}

		// Reset turning state
		if (MathF.Abs(Velocity.X) > MaxSpeedToTurn)
			_isTurning = false;



		// Horizontal movement and position updating
		Velocity.X += speedResult;
		Collider.Position += Vector2.UnitX * Velocity.X;
		HandleCollision(CollisionAxis.Horizontal);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(
			_texture, 
			Collider.Position + Vector2.UnitY * (Size.Y / 2f - _colliderSizeDiff.Y / 2f),// - Vector2.UnitY * (_colliderSizeDif.Y / 2 + (Size.Y - Size.Y * _sizeMultiplier.Y) / -2), 
			null, 
			Color.Blue, 
			_rotation, 
			Pivot, 
			Size * _sizeMultiplier, 
			SpriteEffects.None, 
			0.1f);
	}



	float Approach(float value, float finalValue, float max)
	{
		return Math.Clamp(finalValue - value, -max, max);
	}



	enum CollisionAxis
	{
		Vertical,
		Horizontal
	}

	void HandleCollision(CollisionAxis axis)
	{
		foreach (var col in Collider.Colliders)
		{
			if (col.Tag == "Player")
				continue;

			if (Collider.Check(col))
			{
				if (col.Tag == "Spike")
				{
					//new Timeout(250.0f, () => Main.ChangeScene(Main.CurrentSceneIndex));
					_countdown.Lose();
					break;
				}
				else if (col.Tag == "Coin")
				{
					if (!removedCoinThisFrame)
					{
						SceneManager.CurrentScene.RemoveActor(col.Actor);
						Coin.Coins--;
						removedCoinThisFrame = true;

						if (Coin.Coins == 0)
						{
							//new Timeout(1000, () => Main.ChangeScene(Main.CurrentSceneIndex + 1));
							_countdown.NextLevel();
							break;
						}
					}
					continue;  // Don't do collision
				}


				switch (axis)
				{
					case CollisionAxis.Vertical:
						while (Collider.Check(col))
							Collider.Position -= Vector2.UnitY * Gravity * Main.FixedDeltaTime * Main.FixedDeltaTime * Math.Sign(Velocity.Y);

						if (Velocity.Y > 0)
							IsGrounded = true;
						Velocity.Y = 0;
						break;


					case CollisionAxis.Horizontal:
						while (Collider.Check(col))
							Collider.Position -= Vector2.UnitX * 0.1f * Math.Sign(Velocity.X);
						Velocity.X = 0;
						break;
				}				
				break;
			}
		}
	}


	#region Button Functions
	public bool GetAnyJumpKey()
	{
		for (int i = 0; i < _jumpKeys.Length; i++)
			if (Input.GetKey(_jumpKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyJumpKeyUp()
	{
		for (int i = 0; i < _jumpKeys.Length; i++)
			if (Input.GetKeyUp(_jumpKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyJumpKeyDown()
	{
		for (int i = 0; i < _jumpKeys.Length; i++)
			if (Input.GetKeyDown(_jumpKeys[i]))
				return true;
		return false;
	}



	public bool GetAnyLeftKey()
	{
		for (int i = 0; i < _moveLeftKeys.Length; i++)
			if (Input.GetKey(_moveLeftKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyLeftKeyDown()
	{
		for (int i = 0; i < _moveLeftKeys.Length; i++)
			if (Input.GetKeyDown(_moveLeftKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyLeftKeyUp()
	{
		for (int i = 0; i < _moveLeftKeys.Length; i++)
			if (Input.GetKeyUp(_moveLeftKeys[i]))
				return true;
		return false;
	}

	public bool GetAnyRightKey()
	{
		for (int i = 0; i < _moveRightKeys.Length; i++)
			if (Input.GetKey(_moveRightKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyRightKeyDown()
	{
		for (int i = 0; i < _moveRightKeys.Length; i++)
			if (Input.GetKeyDown(_moveRightKeys[i]))
				return true;
		return false;
	}
	public bool GetAnyRightKeyUp()
	{
		for (int i = 0; i < _moveRightKeys.Length; i++)
			if (Input.GetKeyUp(_moveRightKeys[i]))
				return true;
		return false;
	}
	#endregion
}