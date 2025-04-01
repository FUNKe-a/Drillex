using System;
using System.Linq;
using Godot;

namespace drillex.Assets.Entities.LayerManager;

public partial class LayerManager : Node2D
{
    [Signal]
    public delegate void RotationChangedEventHandler(int rotationID);
    
    [Export] public int XBoundary;
    [Export] public int YBoundary;
    [Export] public Wallet WalletResource { get; set; }

    Pair<bool, TileType> [,] _occupiedPositions;
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
        
        _occupiedPositions = new Pair<bool, TileType>[XBoundary, YBoundary];
        Enumerable.Range(0, XBoundary).ToList().ForEach(x =>
            Enumerable.Range(0, YBoundary).ToList().ForEach(y =>
                _occupiedPositions[x, y] = new Pair<bool, TileType>(false, default(TileType))
            )
        );
        
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
        {
            _rotationID = TileRotation(_rotationID, 1);
            EmitSignal(SignalName.RotationChanged, _rotationID);
        }
    }

    public void AddTile(TileType tileType)
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
        
        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 && 
            !_occupiedPositions[mapPosition.X, mapPosition.Y].First)
        {
            switch (tileType)
            {
                case TileType.Conveyor :
                    if (BuyTile(20))
                    {
                        _conveyorLayer.AddConveyor(mapPosition, _rotationID);
                        _occupiedPositions[mapPosition.X, mapPosition.Y].Second = TileType.Conveyor;
                    }
                    break;
                case TileType.Dropper :
                    if (BuyTile(60))
                    {
                        _dropperLayer.AddDropper(mapPosition, _rotationID);
                        _occupiedPositions[mapPosition.X, mapPosition.Y].Second = TileType.Dropper;
                    }
                    break;
                case TileType.Furnace :
                    if (BuyTile(60))
                    {
                        _conveyorLayer.AddConveyor(mapPosition, 0, true);
                        _furnaceLayer.AddFurnace(mapPosition);
                        _occupiedPositions[mapPosition.X, mapPosition.Y].Second = TileType.Furnace;
                    }
                    break;
                case TileType.Upgrader :
                    if (BuyTile(50))
                    {
                        _conveyorLayer.AddConveyor(mapPosition, _rotationID);
                        _upgraderLayer.AddUpgrader(mapPosition, _rotationID);
                        _occupiedPositions[mapPosition.X, mapPosition.Y].Second = TileType.Upgrader;
                    }
                    break;
                default :
                    return;
            }
            _occupiedPositions[mapPosition.X, mapPosition.Y].First = true;
        }
    }

    public void RemoveTile()
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
        
        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
            _occupiedPositions[mapPosition.X, mapPosition.Y].First)
        {
            switch (_occupiedPositions[mapPosition.X, mapPosition.Y].Second)
            {
                case TileType.Conveyor:
                    _conveyorLayer.RemoveConveyor(mapPosition);
                    WalletResource.Money += 10;
                    break;
                case TileType.Dropper:
                    _dropperLayer.RemoveDropper(mapPosition);
                    WalletResource.Money += 30;
                    break;
                case TileType.Furnace:
                    _furnaceLayer.RemoveFurnace(mapPosition);
                    _conveyorLayer.RemoveConveyor(mapPosition);
                    WalletResource.Money += 30;
                    break;
                case TileType.Upgrader :
                    _upgraderLayer.RemoveUpgrader(mapPosition);
                    _conveyorLayer.RemoveConveyor(mapPosition);
                    WalletResource.Money += 20;
                    break;
            }
            _occupiedPositions[mapPosition.X, mapPosition.Y].Second = TileType.NotSelected;
            _occupiedPositions[mapPosition.X, mapPosition.Y].First = false;
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

    private bool BuyTile(ulong price)
    {
        if (price <= WalletResource.Money)
        {
            WalletResource.Money -= price;
            return true;
        }
        return false;
    }
}