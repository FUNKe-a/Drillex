using Godot;
namespace drillex.Common.Scripts;

public partial class BackgroundTile : RefCounted
{
    public TileType TileType { get; set; }
    public bool IsOccupied { get; set; }
    public bool IsMineable { get; set; }

    public BackgroundTile()
    {
        TileType = TileType.NotSelected;
        IsOccupied = false;
        IsMineable = false;
    }
}