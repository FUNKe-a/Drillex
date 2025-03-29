using System;
using Godot;

namespace drillex.Assets.Entities.LayerManager;

public partial class LayerManager : Node2D
{
    [Export] public int XBoundary;
    [Export] public int YBoundary;
    [Export] public Wallet Wallet { get; private set; }
    
    bool [,] _occupiedPositions;
    Conveyor.Conveyor _conveyorLayer; 
    Dropper.Dropper _dropperLayer;
    
    Vector2I _conveyorAtlasPosition;
    string _action;
    
    public override void _Ready()
    {
        if (XBoundary == 0)
            XBoundary = (int)Math.Ceiling(GetViewport().GetVisibleRect().Size.X / 32f);
        if (YBoundary == 0)
            YBoundary = (int)Math.Ceiling(GetViewport().GetVisibleRect().Size.Y / 32f);
        _occupiedPositions = new bool[XBoundary, YBoundary];
        _conveyorLayer = GetNode<Conveyor.Conveyor>("Conveyor");
        _dropperLayer = GetNode<Dropper.Dropper>("Dropper");
        _conveyorAtlasPosition = Vector2I.Zero;
        _action = "Idle";
    }

    public override void _Process(double delta)
    {
        switch (_action)
        {
            case "Place" :
                AddTile(GetGlobalMousePosition(), GameMenu.SelectedTileType, _conveyorAtlasPosition);
                break;
            case "Delete" :
                RemoveTile(GetGlobalMousePosition());
                break;
            case "Idle" :
                return;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Place"))
            _action = "Place";
        if (@event.IsActionReleased("Place"))
            _action = "Idle";
        if (@event.IsActionPressed("Delete"))
            _action = "Delete";
        if (@event.IsActionReleased("Delete"))
            _action = "Idle";
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionReleased("Rotate"))
        {
            _conveyorAtlasPosition.Y++;
            if (_conveyorAtlasPosition.Y == 4)
                _conveyorAtlasPosition.Y = 0;
        }
    }

    public void AddTile(Vector2 globalMousePosition, TileType tileType, Vector2I atlasPosition)
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
        
        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 && 
            !_occupiedPositions[mapPosition.X, mapPosition.Y])
        {
            switch (tileType)
            {
                case TileType.Conveyor :
                    _conveyorLayer.AddConveyor(mapPosition, atlasPosition);
                    break;
                case TileType.Dropper :
                    _dropperLayer.AddDropper(mapPosition, atlasPosition);
                    break;
                case TileType.Furnace :
                    break;
                default :
                    return;
            }
            _occupiedPositions[mapPosition.X, mapPosition.Y] = true;
        }
    }

    public void RemoveTile(Vector2 globalMousePosition)
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());

        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
            _occupiedPositions[mapPosition.X, mapPosition.Y])
        {
            _conveyorLayer.RemoveConveyor(mapPosition);
            _dropperLayer.RemoveDropper(mapPosition);
            _occupiedPositions[mapPosition.X, mapPosition.Y] = false;
        }
    }
}