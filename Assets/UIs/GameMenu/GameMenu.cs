using Godot;
using System;

public partial class GameMenu : CanvasLayer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
    
    public static TileType SelectedTileType { get; private set; } = 0;
    
    private OptionButton  _tileSelectionButton;
    
    public override void _Ready()
    {
        _tileSelectionButton = (OptionButton)GetNode("TopContainer/TileSelectionButton");
    }

    private void OnMainMenuButtonPressed()
    {
        GetTree().ChangeSceneToFile(MainMenuScene);
    }
    
    private void OnTileSelectionButtonItemSelected(int index)
    {
        SelectedTileType = (TileType)index;
    }
}
