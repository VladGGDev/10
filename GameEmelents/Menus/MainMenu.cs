﻿using Menus;
using Microsoft.Xna.Framework;
public class MainMenu : Menu
{
	public MainMenu()
	{
		Button button1 = new()
		{
			NormalColor = Color.Black,
			SelectedColor = Color.White,
			Text = "Start",
			Padding = new Vector2(50f, 0),
			OnInteract = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1)
		};
		Button button2 = new()
		{
			NormalColor = Color.Black,
			SelectedColor = Color.White,
			Text = "Button 2",
			ScreenPivot = MenuPivot.Right,
			Padding = new Vector2(50f, 50f)
		};
		MenuGroup group = new(button1);
		group.AddNode(button2);
		group.AddRightLink(button1, button2);

		CurrentGroup = group;
	}
}
