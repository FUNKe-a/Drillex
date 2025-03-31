using Godot;
using System;
using System.Collections.Generic;

public partial class UpgradeCounter : RefCounted
{
    private Dictionary<Material, int> MaterialsUpgradeCounter;
    private int _maxUpgradeCount;
    
    public UpgradeCounter(int maxUpgradeCount = 2)
    {
        MaterialsUpgradeCounter = new Dictionary<Material, int>();
        _maxUpgradeCount = maxUpgradeCount;
    }

    public void UpgradeMaterial(Material material, float amount)
    {
        MaterialsUpgradeCounter.TryAdd(material, _maxUpgradeCount);

        if (MaterialsUpgradeCounter[material] > 0)
        {
            material.Multiplier += amount;
            MaterialsUpgradeCounter[material]--; 
            GD.Print($"Upgrade material: {material}, {material.Multiplier}");
        }
    }
}
