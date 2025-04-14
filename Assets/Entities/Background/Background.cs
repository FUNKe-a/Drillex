using Godot;
using System;
using drillex.Common.Scripts;

public partial class Background : TileMapLayer
{
    [Signal]
    public delegate void BackgroundMatrixSetEventHandler();

    bool initialized;
    
    private int _xBoundary;
    private int _yBoundary;

    public void Initialize(int xBoundary, int yBoundary)
    {
        _xBoundary = xBoundary;
        _yBoundary = yBoundary;
        initialized = true;
        CreateBackgroundMatrix();
    }

    public void CreateBackgroundMatrix()
    {
        if (!initialized) return;

        BackgroundTile[,] backgroundMatrix = new BackgroundTile[_xBoundary, _yBoundary];
    }
}
