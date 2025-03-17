using Godot;
using System;
using Godot.NativeInterop;

public partial class GameMenu : CanvasLayer
{
    [Export] 
    public ButtonGroup TileSelectionButtons { get; set; }
    [Signal]
    public delegate void TileSelectionButtonPressedEventHandler(Button selectedButton);

    public override void _Ready()
    {
        foreach (BaseButton tempButton in TileSelectionButtons.GetButtons())
            tempButton.Pressed += () => SelectionButtonPressed();
    }

    private void SelectionButtonPressed()
    {
        BaseButton pressedButton = TileSelectionButtons.GetPressedButton();
        EmitSignal(SignalName.TileSelectionButtonPressed, pressedButton);
    }
}
