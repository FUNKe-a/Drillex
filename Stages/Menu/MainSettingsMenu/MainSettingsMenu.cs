using Godot;
using System;

public partial class MainSettingsMenu : VBoxContainer
{
    [Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
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
    private void OnBackButtonUp() => GetTree().ChangeSceneToFile(MainMenuScene);
}
