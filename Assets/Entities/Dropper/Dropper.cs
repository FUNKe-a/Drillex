using Godot;
using Godot.Collections;

namespace drillex.Assets.Entities.Dropper;

public partial class Dropper : TileMapLayer
{
	[Export] public PackedScene MaterialScene { get; set; }
	[Export] public float DropInterval { get; set; } = 4.0f;
	
	private Node2D _materialHolder;
	private System.Collections.Generic.Dictionary<Vector2I, DropperHolder> _droppers;
	private Vector2I _dropDirection;
	
	public override void _Ready()
	{
		_materialHolder = GetNode<Node2D>("../MaterialHolder");
		_droppers = new System.Collections.Generic.Dictionary<Vector2I, DropperHolder>();
		Array<Vector2I> droppersTemp = GetUsedCells();
		
		foreach (var cellPosition in droppersTemp)
			AddDropperToHolder(cellPosition);
	}

	public override void _PhysicsProcess(double d)
	{
		foreach (var kvp in _droppers)
		{
			var holder = kvp.Value;
			
			holder.TimeElapsed += (float)d;
			if (!holder.IsBlocked && holder.CanMine)
			{
				if (holder.TimeElapsed >= holder.Delay)
				{
					holder.TimeElapsed = 0f;
					DropMaterial(holder.SpawnPosition);
				}
			}
		}
	}

	private void AddDropperToHolder(Vector2I cellPosition)
	{
		TileData tile = GetCellTileData(cellPosition);
		Vector2I dropDirection = tile.GetCustomData("Direction").AsVector2I();
		Vector2I spawnPosition = cellPosition + dropDirection;
		bool isBlocked = _droppers.ContainsKey(spawnPosition);
		DropperHolder dropperAttributes = new DropperHolder(spawnPosition, DropInterval, isBlocked);
		_droppers.Add(cellPosition, dropperAttributes);
	}

	private void UpdateNeighborCellBlockState(Vector2I cellPosition)
	{
		Array<Vector2I> neighborCells = GetSurroundingCells(cellPosition);

		foreach (var neighborPosition in neighborCells)
			if (_droppers.TryGetValue(neighborPosition, out var holder))
				holder.IsBlocked = _droppers.ContainsKey(holder.SpawnPosition);
	}

	public void AddDropper(Vector2I mapPosition, int rotationID, bool canMine)
	{
		SetCell(mapPosition, 0, new Vector2I(0, 0), rotationID);
		AddDropperToHolder(mapPosition);
		UpdateNeighborCellBlockState(mapPosition);
		_droppers[mapPosition].CanMine = canMine;
	}

	public void RemoveDropper(Vector2I mapPosition)
	{
		EraseCell(mapPosition);
		_droppers.Remove(mapPosition);
		UpdateNeighborCellBlockState(mapPosition);
	}

	private void DropMaterial(Vector2I mapLocation)
	{
		if (MaterialScene == null || _materialHolder == null)
			return;

		Material material = (Material)MaterialScene.Instantiate();

		material.Position = MapToLocal(mapLocation) - new Vector2(16,16);
		_materialHolder.AddChild(material);
	}

	public DropperHolder GetDropper(Vector2I mapPos)
	{
		return _droppers[mapPos];
	}
}
