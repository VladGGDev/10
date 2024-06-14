using Menus;
using Menus.MenuElements;
using Microsoft.Xna.Framework;

public class MainMenu : Menu
{
	public MainMenu()
	{
		Button playButton = new()
		{
			Text = "Start",
			Padding = new Vector2(50f, 0),
			OnInteract = () => SceneManager.ChangeScene(SceneManager.CurrentSceneIndex + 1)
		};
		Button button2 = new()
		{
			NormalColor = Color.Black,
			SelectedColor = Color.Cyan,
			Text = "Button 2",
			ScreenPivot = MenuPivot.Right,
			Padding = new Vector2(100f, -50f)
		};
		Slider testSlider = new()
		{
			Text = "Slider",
			Offset = new(0, 150f)
		};
		Text gameTitle = new()
		{
			NormalColor = Color.Transparent,
			String = "10",
			Offset = new(0, -300f),
			Size = new(4f)
		};

		MenuGroup group = new(playButton);
		group.AddNode(gameTitle);

		group.AddNode(button2);
		group.AddRightLink(playButton, button2);
		group.AddDownLink(button2, testSlider, false);

		group.AddNode(testSlider);
		group.AddDownLink(playButton, testSlider);

		CurrentGroup = group;
	}
}
