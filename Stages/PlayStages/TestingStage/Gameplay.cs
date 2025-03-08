using drillex.Assets.Entities.Conveyor;
using drillex.Assets.Entities.Dropper;
using Godot;

namespace drillex.Stages.PlayStages.TestingStage
{
    public partial class Gameplay : Node2D
    {
        Assets.Entities.LayerManager.LayerManager _layerManager;
        private Vector2I _conveyorAtlasPosition;

        public override void _Ready()
        {
            _layerManager = GetNode<Assets.Entities.LayerManager.LayerManager>("LayerManager");
            _conveyorAtlasPosition = new Vector2I(0, 0);
        }

        public override void _Input(InputEvent @event)
        { 
            if (Input.IsActionPressed("Place"))
                _layerManager.AddTile(GetGlobalMousePosition(), TileType.Conveyor, _conveyorAtlasPosition);
            if (Input.IsActionPressed("Place2"))
                _layerManager.AddTile(GetGlobalMousePosition(), TileType.Dropper, _conveyorAtlasPosition);
            
            if (Input.IsActionJustPressed("Rotate"))
            {
                _conveyorAtlasPosition.X++;
                if (_conveyorAtlasPosition.X == 4)
                    _conveyorAtlasPosition.X = 0;
            }
        }
    }
}