using Godot;
using System.Collections.Generic;

namespace drillex.Assets.Entities.Furnace;

public partial class Furnace : TileMapLayer
{
	[Export] private Wallet _wallet;
	
	public void AddFurnace(Vector2I mapPosition) =>
		SetCell(mapPosition, 0, new Vector2I(0, 0), 0);

	public void RemoveFurnace(Vector2I mapPosition) =>
		EraseCell(mapPosition);
}