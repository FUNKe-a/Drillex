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
    
    public override void _Ready()
    {
        _occupiedPositions = new bool[XBoundary, YBoundary];
        _conveyorLayer = GetNode<Conveyor.Conveyor>("Conveyor");
        _dropperLayer = GetNode<Dropper.Dropper>("Dropper");
        _conveyorLayer.Position = TileMapPosition;
        _dropperLayer.Position = TileMapPosition;
    }

    public void AddTile(Vector2 globalMousePosition, TileType tileType, Vector2I atlasPosition)
    {
        Vector2I mapPosition = _conveyorLayer.LocalToMap(_conveyorLayer.ToLocal(globalMousePosition));
        
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
         Vector2I mapPosition = _conveyorLayer.LocalToMap(_conveyorLayer.ToLocal(globalMousePosition));

        if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
            mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
            _occupiedPositions[mapPosition.X, mapPosition.Y])
        {
            _conveyorLayer.EraseCell(mapPosition);
            _dropperLayer.EraseCell(mapPosition);
            _occupiedPositions[mapPosition.X, mapPosition.Y] = false;
        }
    }
}