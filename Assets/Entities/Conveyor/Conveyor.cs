using Godot;
using Godot.Collections;

namespace drillex.Assets.Entities.Conveyor;
public partial class Conveyor : TileMapLayer
{
	[Export] public float TargetPrecision { get; set; } = 0.1f;
	[Export] public float Speed { get; set; }

	private Node2D _materialHolder;
	private Dictionary<Material, MaterialMovementHolder> _materialMovementHolders;
	
	public override void _Ready()
	{
		_materialHolder = GetNode<Node2D>("../MaterialHolder");
		_materialMovementHolders = new Dictionary<Material, MaterialMovementHolder>();
	}

	public override void _PhysicsProcess(double d)
	{
		foreach (var node in _materialHolder.GetChildren())
		{
			Material material = (Material)node;

			if (!_materialMovementHolders.ContainsKey(material))
			{
				_materialMovementHolders.Add(material, new MaterialMovementHolder());
				FindTarget(material, _materialMovementHolders[material]);
			}

			CheckMaterialVelocity(material, _materialMovementHolders[material], out bool targetReached);
			
			if (targetReached) 
				FindTarget(material, _materialMovementHolders[material]);
			
			material.Position += _materialMovementHolders[material].Velocity * (float)d;
		}
	}

	private void FindTarget(Material material, MaterialMovementHolder holder)
	{
		var materialPosition = ToLocal(material.GlobalPosition);
		var mapPosition = LocalToMap(materialPosition);
		var currentTile = GetCellTileData(mapPosition);
		
		holder.Velocity = Vector2.Zero;
		holder.TargetPosition = (Vector2I)materialPosition.Snapped(32f);
		if (currentTile != null)
		{
			Vector2 direction = currentTile.GetCustomData("Direction").AsVector2I();
			Vector2 nextPos = direction * 32 + materialPosition;
			var frontCellAtlas = GetCellAtlasCoords(LocalToMap(nextPos));
			var frontCell = GetCellTileData(LocalToMap(nextPos));
			
			if (frontCell != null)
			{
				if (frontCellAtlas == Vector2.Zero) // if conveyor has a direction
				{
					Vector2 frontCellDirection = frontCell.GetCustomData("Direction").AsVector2I();
					if (direction.Dot(frontCellDirection) >= -0.95f) // if direction is not opposite
					{
						holder.Velocity = direction * Speed;
						holder.TargetPosition = (Vector2I)nextPos;
					}
				}
				else if (frontCellAtlas == Vector2.Down) // if a conveyor doesn't have a direction. Used for furnace
				{
					holder.Velocity = direction * Speed;
					holder.TargetPosition = (Vector2I)nextPos;				
				}
			}
		}
	}

	private void CheckMaterialVelocity(Material material, MaterialMovementHolder holder, out bool targetReached)
	{
		targetReached = false;
		if (material.Position.Snapped(TargetPrecision).IsEqualApprox(holder.TargetPosition))
		{
			material.Position = material.Position.Snapped(32);
			targetReached = true;
			holder.Velocity = Vector2.Zero;
		}
	}

	public void AddConveyor(Vector2I mapPosition, int alternativeTile, bool noDirection = false)
	{
		Vector2I atlasPos = noDirection == false ? Vector2I.Zero : Vector2I.Down;
		SetCell(mapPosition, 0, atlasPos, alternativeTile);
	}

	public void RemoveConveyor(Vector2I mapPosition)
	{
		EraseCell(mapPosition);
	}
}
