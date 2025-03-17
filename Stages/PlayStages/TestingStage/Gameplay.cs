using Godot;

namespace drillex.Stages.PlayStages.TestingStage
{
    public partial class Gameplay : Node2D
    {
        Assets.Entities.LayerManager.LayerManager _layerManager;
        private Vector2I _conveyorAtlasPosition;
        private TileType _selectedTileType;

        public override void _Ready()
        {
            _layerManager = GetNode<Assets.Entities.LayerManager.LayerManager>("LayerManager");
            _conveyorAtlasPosition = new Vector2I(0, 0);
        }

        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionPressed("Place"))
                _layerManager.AddTile(GetGlobalMousePosition(), _selectedTileType, _conveyorAtlasPosition);
            if (Input.IsActionPressed("Delete"))
                _layerManager.RemoveTile(GetGlobalMousePosition());
             
            if (Input.IsActionJustPressed("Rotate"))
            {
                _conveyorAtlasPosition.Y++;
                if (_conveyorAtlasPosition.Y == 4)
                    _conveyorAtlasPosition.Y = 0;
            }
        }

        private void TileSelectionButtonPressed(BaseButton pressedButton)
        {
            if (pressedButton != null)
            {
                switch (pressedButton.Name)
                {
                    case "MiningDrillButton":
                        _selectedTileType = TileType.Dropper;
                        break;
                    case "ConveyorButton":
                        _selectedTileType = TileType.Conveyor;
                        break;
                    default:
                        _selectedTileType = TileType.NotSelected;
                        break;
                }
            }
            else _selectedTileType = TileType.NotSelected;
        }
    }
}