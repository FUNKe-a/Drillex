using Godot;
using System;
using System.Collections.Generic;

public partial class UpgradeCounter : RefCounted
{
    private Dictionary<Material, int> _materialsUpgradeCounter;
    private int _maxUpgradeCount;
    
    public UpgradeCounter(int maxUpgradeCount = 2)
    {
        _materialsUpgradeCounter = new Dictionary<Material, int>();
        _maxUpgradeCount = maxUpgradeCount;
    }
    
    public void UpgradeMaterial(Material material, float amount)
    {
        _materialsUpgradeCounter.TryAdd(material, _maxUpgradeCount);

        if (_materialsUpgradeCounter[material] > 0)
        {
            material.Multiplier += amount;
            _materialsUpgradeCounter[material]--; 
        }
    }
}
