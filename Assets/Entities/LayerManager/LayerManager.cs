using Godot;
using System;
using Godot.Collections;

public partial class LayerManager : Node2D
{
    [Export] public int XBoundary;
    [Export] public int YBoundary;
    private bool [,] _occupiedPositions;

    public override void _Ready()
    {
        _occupiedPositions = new bool[XBoundary - 1, YBoundary - 1];
    }
}
