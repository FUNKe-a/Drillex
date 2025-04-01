using System;
using Godot;

namespace drillex.Assets.Entities.LayerManager;

public partial class LayerManager : Node2D
{
    [Export] public int XBoundary;
    [Export] public int YBoundary;
    
    bool [,] _occupiedPositions;
    Conveyor.Conveyor _conveyorLayer; 
    Dropper.Dropper _dropperLayer;
    Furnace.Furnace _furnaceLayer;
    Upgrader.Upgrader _upgraderLayer;

    private int _rotationID;
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
        _furnaceLayer = GetNode<Furnace.Furnace>("Furnace");
        _upgraderLayer = GetNode<Upgrader.Upgrader>("Upgrade");
        _rotationID = 0;
        _action = "Idle";
    }

    public override void _Process(double delta)
    {
        switch (_action)
        {
            case "Place" :
                AddTile(GameMenu.SelectedTileType);
                break;
            case "Delete" :
                RemoveTile();
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
            _rotationID = TileRotation(_rotationID, 1);
    }

    public void AddTile(TileType tileType)
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
        
        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 && 
            !_occupiedPositions[mapPosition.X, mapPosition.Y])
        {
            switch (tileType)
            {
                case TileType.Conveyor :
                    _conveyorLayer.AddConveyor(mapPosition, _rotationID);
                    break;
                case TileType.Dropper :
                    _dropperLayer.AddDropper(mapPosition, _rotationID);
                    break;
                case TileType.Furnace :
                    _conveyorLayer.AddConveyor(mapPosition, 0, true);
                    _furnaceLayer.AddFurnace(mapPosition);
                    break;
                case TileType.Upgrader :
                    _conveyorLayer.AddConveyor(mapPosition, _rotationID);
                    _upgraderLayer.AddUpgrader(mapPosition, _rotationID);
                    break;
                default :
                    return;
            }
            _occupiedPositions[mapPosition.X, mapPosition.Y] = true;
        }
    }

    public void RemoveTile()
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
        
        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
            _occupiedPositions[mapPosition.X, mapPosition.Y])
        {
            _conveyorLayer.RemoveConveyor(mapPosition);
            _dropperLayer.RemoveDropper(mapPosition);
            _furnaceLayer.RemoveFurnace(mapPosition);
            _upgraderLayer.RemoveUpgrader(mapPosition);
            _occupiedPositions[mapPosition.X, mapPosition.Y] = false;
        }
    }

    private int TileRotation(int rotationID, int value)
    {
        int returnID = rotationID;
        
        for (int i = 0; i < value; i++)
        {
            returnID++;
            if (returnID == 4)
                returnID = 0;
        }

        return returnID;
    }
}