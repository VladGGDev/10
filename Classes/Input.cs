using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

public static class Input
{
	static KeyboardState _prevKeyState = new KeyboardState();
	static bool[] _prevMouseState = new bool[5];
	static int _prevMouseScroll = 0;

	internal static void _UpdatePrevInput()
	{
		UpdatePrevKeyState();
		UpdatePrevMouseState();
	}

	static void UpdatePrevKeyState()
	{
		_prevKeyState = Keyboard.GetState();
	}

	static void UpdatePrevMouseState()
	{
		_prevMouseState[0] = Mouse.GetState().LeftButton == ButtonState.Pressed;
		_prevMouseState[1] = Mouse.GetState().RightButton == ButtonState.Pressed;
		_prevMouseState[2] = Mouse.GetState().MiddleButton == ButtonState.Pressed;
		_prevMouseState[3] = Mouse.GetState().XButton1 == ButtonState.Pressed;
		_prevMouseState[4] = Mouse.GetState().XButton2 == ButtonState.Pressed;

		_prevMouseScroll = Mouse.GetState().ScrollWheelValue;
	}

	static bool IntToMouseButton(int value) => value switch
	{
		0 => Mouse.GetState().LeftButton == ButtonState.Pressed,
		1 => Mouse.GetState().RightButton == ButtonState.Pressed,
		2 => Mouse.GetState().MiddleButton == ButtonState.Pressed,
		3 => Mouse.GetState().XButton1 == ButtonState.Pressed,
		4 => Mouse.GetState().XButton2 == ButtonState.Pressed,
		_ => throw new ArgumentException("There are only 5 possible values")
	};


	public static float MouseScrollDelta
	{
		get
		{
			float result = (Mouse.GetState().ScrollWheelValue - _prevMouseScroll) / 120f;
			return result;
		}
	}

	/// <summary>
	/// <para>
	/// 0 - left click
	/// </para>
	/// <para>
	/// 1 - right click
	/// </para>
	/// <para>
	/// 2 - middle click
	/// </para>
	/// <para>
	/// 3 - lower thumb button
	/// </para>
	/// <para>
	/// 4 - upper thumb button
	/// </para>
	/// </summary>
	public static bool GetMouseButtonDown(int button)
	{
		return IntToMouseButton(button) && !_prevMouseState[button];
	}

	/// <summary>
	/// <para>
	/// 0 - left click
	/// </para>
	/// <para>
	/// 1 - right click
	/// </para>
	/// <para>
	/// 2 - middle click
	/// </para>
	/// <para>
	/// 3 - lower thumb button
	/// </para>
	/// <para>
	/// 4 - upper thumb button
	/// </para>
	/// </summary>
	public static bool GetMouseButtonUp(int button)
	{
		return !IntToMouseButton(button) && _prevMouseState[button];
	}

	/// <summary>
	/// <para>
	/// 0 - left click
	/// </para>
	/// <para>
	/// 1 - right click
	/// </para>
	/// <para>
	/// 2 - middle click
	/// </para>
	/// <para>
	/// 3 - lower thumb button
	/// </para>
	/// <para>
	/// 4 - upper thumb button
	/// </para>
	/// </summary>
	public static bool GetMouseButton(int button)
	{
		return IntToMouseButton(button);
	}

	public static bool GetKeyDown(Keys key)
	{
		return Keyboard.GetState().IsKeyDown(key) && !_prevKeyState.IsKeyDown(key);
	}

	public static bool GetKeyUp(Keys key)
	{
		return Keyboard.GetState().IsKeyUp(key) && !_prevKeyState.IsKeyUp(key);
	}

	public static bool GetKey(Keys key)
	{
		return Keyboard.GetState().IsKeyDown(key);
	}
}