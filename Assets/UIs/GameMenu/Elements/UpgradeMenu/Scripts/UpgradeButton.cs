using Godot;
using System;

public partial class UpgradeButton : Godot.Button
{
    private Action _cachedButtonPressedAction;
    
    public void ConnectToUpgradeButtonPressed(Action uponButtonPress)
    {
        if (_cachedButtonPressedAction is not null)
            Pressed -= _cachedButtonPressedAction;
        
        Pressed += uponButtonPress;
        _cachedButtonPressedAction = uponButtonPress;
    }
}
