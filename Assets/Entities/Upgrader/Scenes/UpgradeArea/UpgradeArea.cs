using Godot;
using System.Collections.Generic;

public partial class UpgradeArea : Area2D
{
    [Export] public float IncreaseMultiplier { get; private set; }
    [Export] public int PassAmount { get; private set; }
    
    private Dictionary<ulong, int> _upgradeCounts;

    public override void _Ready() =>
        _upgradeCounts = new Dictionary<ulong, int>();

    private void OnAreaEntered(Area2D body)
    {
        if (body is Material mat)
        {
            ulong oreId = mat.GetInstanceId();
            
            if (_upgradeCounts.TryAdd(oreId, 0))
                mat.TreeExited += () => OnMaterialExited(oreId);
            
            if (_upgradeCounts[oreId] < PassAmount) { 
                mat.Multiplier += IncreaseMultiplier;
                _upgradeCounts[oreId]++;
            }
        }
    }
    
    private void OnMaterialExited(ulong oreId) =>
        _upgradeCounts.Remove(oreId);
}