using Godot;

namespace drillex.Assets.Entities.LayerManager;

public partial class LayerManager : Node2D
{
    [Export] public int XBoundary;
    [Export] public int YBoundary;
    [Export] public Vector2I TileMapPosition;
    
    bool [,] _occupiedPositions;
    Conveyor.Conveyor _conveyorLayer;
    Dropper.Dropper _dropperLayer;
    
    private Vector2I _conveyorAtlasPosition;

    private string Action;
    
    public override void _Ready()
    {
        _occupiedPositions = new bool[XBoundary, YBoundary];
        _conveyorLayer = GetNode<Conveyor.Conveyor>("Conveyor");
        _dropperLayer = GetNode<Dropper.Dropper>("Dropper");
        _conveyorLayer.Position = TileMapPosition;
        _dropperLayer.Position = TileMapPosition;
        _conveyorAtlasPosition = Vector2I.Zero;
        Action = "Idle";
    }

    public override void _Process(double delta)
    {
        switch (Action)
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
            Action = "Place";
        if (@event.IsActionReleased("Place"))
            Action = "Idle";
        if (@event.IsActionPressed("Delete"))
            Action = "Delete";
        if (@event.IsActionReleased("Delete"))
            Action = "Idle";
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
        Vector2I mapPosition = _conveyorLayer.LocalToMap(_conveyorLayer.ToLocal(globalMousePosition));
        
        if (!_occupiedPositions[mapPosition.X, mapPosition.Y] &&
            mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0)
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
         Vector2I mapPosition = _conveyorLayer.LocalToMap(_conveyorLayer.ToLocal(globalMousePosition));

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