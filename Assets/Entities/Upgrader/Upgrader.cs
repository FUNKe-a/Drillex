using System.Collections.Generic;
using Godot;

namespace drillex.Assets.Entities.Upgrader;

public partial class Upgrader : TileMapLayer
{
    Node2D _materialHolder;
    private Dictionary<Vector2, UpgradeCounter> _upgradeCounter;
    private Dictionary<Material, Pair<Vector2, bool>> _vectorPos;

    public override void _Ready()
    {
        _vectorPos = new Dictionary<Material, Pair<Vector2, bool>>();
        _materialHolder = GetNode<Node2D>("../MaterialHolder");
        _upgradeCounter = new Dictionary<Vector2, UpgradeCounter>();
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var node in _materialHolder.GetChildren())
        {
            Material material = (Material)node;
            Vector2 materialPos = FloorBy32(material.Position);
            
            if (!_vectorPos.TryAdd(material, new Pair<Vector2, bool>(materialPos, true)))
                CheckLastMatLocation(material);
            
            if (_upgradeCounter.TryGetValue(materialPos, out var counter) && 
                _vectorPos[material].Second)
            {
                counter.UpgradeMaterial(material, 0.1f);
                _vectorPos.TryGetValue(material, out var lastPos);
                lastPos.First = materialPos;
                lastPos.Second = false;
            }
        }
    }
	
    public void AddUpgrader(Vector2I mapPosition, int rotationID)
    {
        SetCell(mapPosition, 0, new Vector2I(0, 0), rotationID);
        _upgradeCounter.Add( MapToLocal(mapPosition) - new Vector2I(16, 16), new UpgradeCounter());
    }

    public void RemoveUpgrader(Vector2I mapPosition)
    {
        EraseCell(mapPosition);
        _upgradeCounter.Remove(MapToLocal(mapPosition) - new Vector2I(16, 16));
    }

    private void CheckLastMatLocation(Material material)
    {
        if (_vectorPos.TryGetValue(material, out var lastPos))
        {
            if (lastPos.First.X + 32 < material.Position.X || material.Position.X < lastPos.First.X ||
                lastPos.First.Y + 32 < material.Position.Y || material.Position.Y < lastPos.First.Y)
            {
                lastPos.Second = true;
                lastPos.First = FloorBy32(material.Position);
            }
        }
    }

    private Vector2 FloorBy32(Vector2 vector)
    {
        Vector2 returnVector = new Vector2();
        returnVector.X = (int)(vector.X / 32) * 32;
        returnVector.Y = (int)(vector.Y / 32) * 32;
        
        return returnVector;
    }
}