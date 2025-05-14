using Godot;

public partial class Material : Area2D
{
    private ulong _monetaryValue;
    private float _multiplier;
    public uint UpgradeCount { get; set; }
    
    [Export]
    public ulong MonetaryValue
    {
        get => (ulong)Mathf.Ceil(_monetaryValue * Multiplier);
        set => _monetaryValue = value;
    }

    public float Multiplier
    {
        get => _multiplier;
        set => _multiplier = value >= 0 ? value : _multiplier;
    }
    
    public override void _Ready()
    {
        UpgradeCount = 0;
        Multiplier = 1.0f;
        Position = Position.Snapped(32f);
    }
}