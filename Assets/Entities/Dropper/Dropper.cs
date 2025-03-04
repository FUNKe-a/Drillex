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
		[Export] public Node2D Holder { get; set; }
		private List<DropperHolder> _dropperHolders;

		private float _timeElapsed;
		private Vector2I _dropDirection;

		public override void _Ready()
		{
			_dropperHolders = new List<DropperHolder>();
			Array<Vector2I> droppers = GetUsedCells();
			
			foreach (var cellPosition in droppers)
				AddDropperToHolder(cellPosition);
		}

		public override void _PhysicsProcess(double d)
		{
			Vector2I[] newDroppers = GetUsedCells().ToArray().ExceptBy(_dropperHolders.Select(x => x.MapPosition), y => y).ToArray();
			foreach (var dropper in newDroppers)
				AddDropperToHolder(dropper);
			
			foreach (DropperHolder holder in _dropperHolders)
			{
				holder.TimeElapsed += (float)d;
				if (holder.TimeElapsed >= holder.Delay)
				{
					holder.TimeElapsed = 0f;
					DropMaterial(holder.SpawnPosition);
				}
			}
		}

		private void AddDropperToHolder(Vector2I cellPosition)
		{
			Vector2I atlasCoordinates = GetCellAtlasCoords(cellPosition);
			int directionIndex = atlasCoordinates.X;
			int delay = 1;
					
			Vector2I dropDirection = directionIndex switch
			{
				0 => Vector2I.Up,
				1 => Vector2I.Down,
				2 => Vector2I.Left,
				3 => Vector2I.Right,
				_ => Vector2I.Zero
			};
					
			Vector2I spawnPosition = cellPosition + dropDirection;
			DropperHolder dropperAttributes = new DropperHolder(spawnPosition, cellPosition, delay);
			_dropperHolders.Add(dropperAttributes);
		}

		public void AddDropper(Vector2 globalPosition, Vector2I dropperAtlasPosition)
		{
			Vector2 localPosition = ToLocal(globalPosition);
			Vector2I tilePosition = LocalToMap(localPosition);

			SetCell(tilePosition, 0, dropperAtlasPosition);
		}

		private void DropMaterial(Vector2I mapLocation)
		{
			if (MaterialScene == null || Holder == null)
				return;

			Material material = (Material)MaterialScene.Instantiate();

			material.Position = MapToLocal(mapLocation) - new Vector2(16,16);
			Holder.AddChild(material);
		}
	}
}
