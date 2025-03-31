using Godot;

namespace drillex.Assets.Entities.Upgrader;

public partial class Upgrader : TileMapLayer
{
    [Export] private Wallet _wallet;
	
    Node2D _materialHolder;
    private System.Collections.Generic.Dictionary<Vector2, UpgradeCounter> _upgradeCounter;


    public override void _Ready()
    {
        _materialHolder = GetNode<Node2D>("../MaterialHolder");
        _upgradeCounter = new System.Collections.Generic.Dictionary<Vector2, UpgradeCounter>();
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var node in _materialHolder.GetChildren())
        {
            Material material = (Material)node;

            if (_upgradeCounter.TryGetValue(material.Position.Snapped(32), out var counter))
            {
                counter.UpgradeMaterial(material, 0.1f);
            }
        }
    }
	
    public void AddUpgrader(Vector2I mapPosition)
    {
        SetCell(mapPosition, 0, new Vector2I(0, 0));
        _upgradeCounter.Add( MapToLocal(mapPosition) - new Vector2I(16, 16), new UpgradeCounter());
    }

    public void RemoveUpgrader(Vector2I mapPosition)
    {
        EraseCell(mapPosition);
        _upgradeCounter.Remove(MapToLocal(mapPosition) - new Vector2I(16, 16));
    }
}