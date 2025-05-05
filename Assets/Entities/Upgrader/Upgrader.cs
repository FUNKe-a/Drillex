using System.Collections.Generic;
using Godot;

namespace drillex.Assets.Entities.Upgrader;

public partial class Upgrader : TileMapLayer
{
    private Dictionary<Vector2I, UpgradeArea> _sceneCoords;
    
    public override void _Ready()
    {
        _sceneCoords = new Dictionary<Vector2I, UpgradeArea>();
        ChildEnteredTree += OnChildEnteredTree;
        ChildExitingTree += OnChildExitedTree;
    }

    public void AddUpgrader(Vector2I mapPosition) =>
        SetCell(mapPosition, 0, new Vector2I(0, 0), 0);

    public void RemoveUpgrader(Vector2I mapPosition) =>
        EraseCell(mapPosition);

    public UpgradeArea GetUpgrader(Vector2I mapPosition) =>
        _sceneCoords[mapPosition];

    private async void OnChildEnteredTree(Node node)
    {
        if (node is UpgradeArea upgradeArea)
        {
            await ToSignal(upgradeArea, "ready");
            var coords = LocalToMap(ToLocal(upgradeArea.GlobalPosition));
            _sceneCoords.Add(coords, upgradeArea);
            upgradeArea.SetMeta("TileCoords", coords);
        }
    }

    private void OnChildExitedTree(Node node)
    {
        if (node is UpgradeArea upgradeArea)
            _sceneCoords.Remove((Vector2I)upgradeArea.GetMeta("TileCoords"));
    }
}