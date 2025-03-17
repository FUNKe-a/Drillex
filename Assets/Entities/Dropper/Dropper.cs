using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

namespace drillex.Assets.Entities.Dropper
{
	public partial class Dropper : TileMapLayer
	{
		[Export] public PackedScene MaterialScene { get; set; }
		[Export] public float DropInterval { get; set; } = 1.0f;

		private Node2D _materialHolder;
		private System.Collections.Generic.Dictionary<Vector2I, DropperHolder> _dropperHolders;
		private float _timeElapsed;
		private Vector2I _dropDirection;

		public override void _Ready()
		{
			_materialHolder = GetNode<Node2D>("../MaterialHolder");
			_dropperHolders = new System.Collections.Generic.Dictionary<Vector2I, DropperHolder>();
			Array<Vector2I> droppers = GetUsedCells();
			
			foreach (var cellPosition in droppers)
				AddDropperToHolder(cellPosition);
		}

		public override void _PhysicsProcess(double d)
		{
			/*Vector2I[] newDroppers = GetUsedCells().ExceptBy(_dropperHolders.Select(x => x.MapPosition), y => y).ToArray();
			foreach (var dropper in newDroppers)
				AddDropperToHolder(dropper);*/
			
			foreach (var kvp in _dropperHolders)
			{
				var holder = kvp.Value;
				
				holder.TimeElapsed += (float)d;
				if (!holder.IsBlocked)
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
			Vector2I atlasCoordinates = GetCellAtlasCoords(cellPosition);
			int directionIndex = atlasCoordinates.Y;
			int delay = 1;
					
			Vector2I dropDirection = directionIndex switch
			{
				0 => Vector2I.Up,
				1 => Vector2I.Right,
				2 => Vector2I.Down,
				3 => Vector2I.Left,
				_ => Vector2I.Zero
			};
					
			Vector2I spawnPosition = cellPosition + dropDirection;
			bool isBlocked = _dropperHolders.ContainsKey(spawnPosition);
			DropperHolder dropperAttributes = new DropperHolder(spawnPosition, delay, isBlocked);
			_dropperHolders.Add(cellPosition, dropperAttributes);
		}

		private void UpdateNeighborCellBlockState(Vector2I cellPosition)
		{
			Array<Vector2I> neighborCells = GetSurroundingCells(cellPosition);

			foreach (var neighborPosition in neighborCells)
				if (_dropperHolders.TryGetValue(neighborPosition, out var holder))
					holder.IsBlocked = _dropperHolders.ContainsKey(holder.SpawnPosition);
		}

		public void AddDropper(Vector2I mapPosition, Vector2I dropperAtlasPosition)
		{
			SetCell(mapPosition, 0, dropperAtlasPosition);
			AddDropperToHolder(mapPosition);
			UpdateNeighborCellBlockState(mapPosition);
		}

		public void RemoveDropper(Vector2I mapPosition)
		{
			EraseCell(mapPosition);
			_dropperHolders.Remove(mapPosition);
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
	}
}
