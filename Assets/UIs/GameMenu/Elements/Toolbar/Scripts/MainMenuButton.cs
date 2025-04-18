using Godot;

public partial class MainMenuButton : Button
{
    [Export(PropertyHint.File, "*.tscn")] public string LocationUponPress;
    public override void _Ready()
    {
        ButtonUp += ChangeScene;
    }

    private void ChangeScene()
    {
        GetTree().ChangeSceneToFile(LocationUponPress);
    }
}
