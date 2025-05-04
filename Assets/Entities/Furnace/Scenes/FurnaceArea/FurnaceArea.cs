using System.Collections.Generic;
using Godot;

public partial class FurnaceArea : Area2D
{
    [Export] public Wallet Wallet;
    
    private List<Material> _currList;
    
    public override void _Ready() =>
        _currList = new List<Material>();

    public override void _Process(double delta)
    {
        foreach (var node in _currList)
        {
            if (node.GlobalPosition.IsEqualApprox(GlobalPosition - new Vector2(16, 16)))
            {
                node.QueueFree();
                Wallet.AddMoney(node.MonetaryValue);
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Material mat)
        {
            if (!_currList.Contains(mat))
            {
                _currList.Add(mat);
                mat.TreeExited += () => OnMaterialExited(mat);
            }
        }
    }
    
    private void OnMaterialExited(Material material) =>
        _currList.Remove(material);
}
