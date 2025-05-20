using Godot;
using System;

public partial class MainSettingsMenu : VBoxContainer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	[Export(PropertyHint.File, "*.tscn")] public string KeybindMenuScene;

	public override void _Ready()
	{
		switch (GetWindow().Size)
		{
			case (960, 544) :
				GetNode<OptionButton>("ResolutionButton").Selected = 0;
				break;
			case (1920, 1088) :
				GetNode<OptionButton>("ResolutionButton").Selected = 1;
				break;
		}
	}

    private void OnResolutionButtonItemSelected(int index)
    {
        switch (index)
        {
            case 0 :
                GetWindow().Size = new Vector2I(960, 544);
                break;
            case 1 :
                GetWindow().Size = new Vector2I(1920, 1088);
                break;
        }
    }

    private void OnBackButtonUp() =>
        QueueFree();

    private void OnKeybindMenuButtonUp()
    {
	    var scene = GD.Load<PackedScene>(KeybindMenuScene);
	    Visible = false;
	    var instance = scene.Instantiate<KeybindSettingsMenu>();
	    instance.TreeExited += () => Visible = true;
	    AddSibling(instance);
    }
}
