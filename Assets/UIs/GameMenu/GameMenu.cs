using Godot;
using System;
using Godot.NativeInterop;

public partial class GameMenu : CanvasLayer
{
    [Export] 
    public ButtonGroup TileSelectionButtons { get; set; }
    [Signal]
    public delegate void TileSelectionButtonPressedEventHandler(int selectedButton);

    public override void _Ready()
    {
        foreach (BaseButton tempButton in TileSelectionButtons.GetButtons())
            tempButton.Pressed += SelectionButtonPressed;
    }

    private void SelectionButtonPressed()
    {
        BaseButton pressedButton = TileSelectionButtons.GetPressedButton();
        TileType selectedTileType = TileType.NotSelected;
        if (pressedButton != null)
        {
            switch (pressedButton.Name)
            {
                case "MiningDrillButton":
                    selectedTileType = TileType.Dropper;
                    break;
                case "ConveyorButton":
                    selectedTileType = TileType.Conveyor;
                    break;
                default:
                    selectedTileType = TileType.NotSelected;
                    break;
            }
        }
        EmitSignal(SignalName.TileSelectionButtonPressed, (int)selectedTileType);
    }
}
