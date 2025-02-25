using Godot;

public partial class Material : Node2D
{
    public Vector2I PreviousDirection = Vector2I.Zero;
    public override void _Ready()
    {
        Position = Position.Snapped(32f);
    }

    public override void _PhysicsProcess(double delta)
    {

    }
}