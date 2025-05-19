using Godot;
using System;

public partial class PauseMenu : Control
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	public override void _Ready(){
		SetProcessMode(ProcessModeEnum.WhenPaused);

		var resumeButton = GetNode<Button>("VBoxContainer/ResumeButton");
		var restartButton = GetNode<Button>("VBoxContainer/RestartButton");
		var settingsButton = GetNode<Button>("VBoxContainer/SettingsButton");
		var mainMenuButton = GetNode<Button>("VBoxContainer/MainMenuButton");

		resumeButton.Pressed += OnResumeButtonPressed;
		restartButton.Pressed += OnRestartButtonPressed;
		settingsButton.Pressed += OnSettingsButtonPressed;
		mainMenuButton.Pressed += OnMainMenuButtonPressed;

		GetTree().Paused = true;
	}
	
	private void OnResumeButtonPressed(){
		GetTree().Paused = false;
		QueueFree();
	}

	private void OnRestartButtonPressed()
	{
		GetTree().Paused=false;
		GetTree().ReloadCurrentScene();
	}
	private void OnSettingsButtonPressed()
	{
		VBoxContainer box = GetNode<VBoxContainer>("VBoxContainer");
		box.Visible=false;
		var optionsScene = GD.Load<PackedScene>("res://Stages/Menu/MainSettingsMenu/MainSettingsMenu.tscn");
		var instance = optionsScene.Instantiate();

		if (instance is MainSettingsMenu settingsMenu)
		{
			settingsMenu.OnClosed = () =>
			{
				box.Visible = true;
			};
		}

		AddChild(instance, true);
	}

	private void OnMainMenuButtonPressed(){
		GetTree().Paused=false;
		GetTree().ChangeSceneToFile(MainMenuScene);
	}
}
