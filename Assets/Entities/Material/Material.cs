using Godot;

public partial class Material : Sprite2D
{
    [Export] public ulong MonetaryValue { get; set; }
    
    public override void _Ready()
    {
        Position = Position.Snapped(32f);
    }

    public override void _PhysicsProcess(double delta)
    {

    }
}