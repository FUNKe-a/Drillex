using drillex.Assets.Entities.Dropper;
using Godot;

namespace drillex.Stages.PlayStages.TestingStage
{
    public partial class Gameplay : Node2D
    {
        private bool _isMouseButtonLeftPressed;
        private bool _isMouseButtonRightPressed;
        private Conveyor _conveyor;
        private Dropper _dropper;
        private Vector2I _conveyorAtlasPosition;

        public override void _Ready()
        {
            _conveyorAtlasPosition = new Vector2I(0, 0);
            _conveyor = GetNode<Conveyor>("Conveyor");
            _dropper = GetNode<Dropper>("Dropper");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_isMouseButtonLeftPressed)
            {
                Vector2 mousePosition = GetGlobalMousePosition();
                _conveyor.AddConveyor(mousePosition, _conveyorAtlasPosition);
            }

            if (_isMouseButtonRightPressed)
            {
                Vector2 mousePosition = GetGlobalMousePosition();
                _dropper.AddDropper(mousePosition, _conveyorAtlasPosition);
            }
        }

        public override void _Input(InputEvent @event)
        { 
            _isMouseButtonLeftPressed = Input.IsActionPressed("Place"); 
            _isMouseButtonRightPressed = Input.IsActionPressed("Place2");

            if (Input.IsActionJustPressed("Rotate"))
            {
                _conveyorAtlasPosition.X++;
                if (_conveyorAtlasPosition.X == 4)
                    _conveyorAtlasPosition.X = 0;
            }
        }
    }
}