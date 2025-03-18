using Godot;
using System;

public partial class MainSettingsMenu : VBoxContainer
{
    [Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;

    public override void _Ready()
    {
        switch (GetWindow().Size)
        {
            case (960, 540) :
                GetNode<OptionButton>("ResolutionButton").Selected = 0;
                break;
            case (1920, 1080) :
                GetNode<OptionButton>("ResolutionButton").Selected = 1;
                break;
        }
    }

    private void OnResolutionButtonItemSelected(int index)
    {
        switch (index)
        {
            case 0 :
                GetWindow().Size = new Vector2I(960, 540);
                break;
            case 1 :
                GetWindow().Size = new Vector2I(1920, 1080);
                break;
        }
    }
    private void OnBackButtonUp() => GetTree().ChangeSceneToFile(MainMenuScene);
}
