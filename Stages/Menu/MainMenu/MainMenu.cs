using Godot;
using System;

public partial class MainMenu : VBoxContainer
{
	[Export(PropertyHint.File, "*.tscn")] public string PlayScene;
	[Export(PropertyHint.File, "*.tscn")] public string SettingsScene;

	private void OnPlayButtonUp() => GetTree().ChangeSceneToFile(PlayScene);
	private void OnSettingsButtonUp() => GetTree().ChangeSceneToFile(SettingsScene);
	private void OnExitButtonUp() => GetTree().Quit();
}
