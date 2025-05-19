using Godot;
using System;

public partial class MainMenu : VBoxContainer
{
	[Export(PropertyHint.File, "*.tscn")] public string PlayScene;
	[Export(PropertyHint.File, "*.tscn")] public string SettingsScene;

	private void OnPlayButtonUp() => GetTree().ChangeSceneToFile(PlayScene);
	//private void OnSettingsButtonUp() => GetTree().ChangeSceneToFile(SettingsScene);
	private void OnExitButtonUp() => GetTree().Quit();
	
	private void OnSettingsButtonUp()
	{
		VBoxContainer box = GetNode<VBoxContainer>(".");
		box.Visible=false;
		var optionsScene = GD.Load<PackedScene>(SettingsScene);
		MainSettingsMenu instance = (MainSettingsMenu)optionsScene.Instantiate();

		//var settingsMenu = instance as MainSettingsMenu;
		if (instance != null)
		{
			instance.OnClosed = () =>
			{
				box.Visible = true;
			};
		}
		CanvasLayer root = GetNode<CanvasLayer>("..");
		root.AddChild(instance, true);
		GD.Print(instance.Visible);
	}
}
