using Godot;
using System;

public partial class MainSettingsMenu : VBoxContainer
{
    [Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;

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
    public Action OnClosed;
    
    public void OnBackButtonUp()
    {
        OnClosed?.Invoke();
        QueueFree();
    }
}
