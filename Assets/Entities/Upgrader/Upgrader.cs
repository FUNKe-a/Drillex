using System.Collections.Generic;
using Godot;

namespace drillex.Assets.Entities.Upgrader;

public partial class Upgrader : TileMapLayer
{
    public void AddUpgrader(Vector2I mapPosition) =>
        SetCell(mapPosition, 0, new Vector2I(0, 0), 0);

    public void RemoveUpgrader(Vector2I mapPosition) =>
        EraseCell(mapPosition);
}