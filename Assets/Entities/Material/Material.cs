using Godot;

public partial class Material : Sprite2D
{
    private ulong _monetaryValue;
    
    [Export]
    public ulong MonetaryValue
    {
        get => (ulong)Mathf.Ceil(_monetaryValue * Multiplier);
        set => _monetaryValue = value;
    }
    
    public float Multiplier { get; set; }
    
    public override void _Ready()
    {
        Multiplier = 1.0f;
        Position = Position.Snapped(32f);
    }

    public override void _PhysicsProcess(double delta)
    {

    }
}