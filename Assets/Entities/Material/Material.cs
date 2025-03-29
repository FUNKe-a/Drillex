using Godot;

public partial class Material : StaticBody2D
{
    public override void _Ready()
    {
        Position = Position.Snapped(32f);
    }

    public override void _PhysicsProcess(double delta)
    {

    }
}