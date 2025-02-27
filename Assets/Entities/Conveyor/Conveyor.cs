using Godot;
using Godot.Collections;

namespace drillex.Assets.Entities.Conveyor;

public partial class Conveyor : TileMapLayer
{
	[Export] 
	public float TargetPrecision { get; set; } = 0.1f;
	[Export]
	public float Speed { get; set; }
	[Export] 
	public Node2D Holder { get; set; }

	private Array<Vector2> _conveyorVelocity = new Array<Vector2>();
	private Array<Vector2I> _conveyorDirection = new Array<Vector2I>();
	private Dictionary<Material, MaterialMovementHolder> _materialMovementHolders = new Dictionary<Material, MaterialMovementHolder>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_conveyorDirection.Add(Vector2I.Up);
		_conveyorDirection.Add(Vector2I.Down);
		_conveyorDirection.Add(Vector2I.Left);
		_conveyorDirection.Add(Vector2I.Right);
		
		_conveyorVelocity.Add(Speed * Vector2.Up); // corresponds to atlas tile 0
		_conveyorVelocity.Add(Speed * Vector2.Down); // corresponds to atlas tile 1
		_conveyorVelocity.Add(Speed * Vector2.Left); // corresponds to atlas tile 2
		_conveyorVelocity.Add(Speed * Vector2.Right); // corresponds to atlas tile 3
	}

	public override void _PhysicsProcess(double d)
	{
		foreach (var node in Holder.GetChildren())
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
        var directionIndex = GetCellAtlasCoords(mapPosition).X;
        
		holder.Velocity = Vector2.Zero;
		if (directionIndex != -1)
		{
			holder.Velocity = _conveyorVelocity[directionIndex];
			holder.TargetPosition = _conveyorDirection[directionIndex] * 32 + (Vector2I)materialPosition;
		}
	}

	private void CheckMaterialVelocity(Material material, MaterialMovementHolder holder, out bool targetReached)
	{
		targetReached = false;
		if (material.Position.Snapped(TargetPrecision).IsEqualApprox(holder.TargetPosition))
		{
			GD.Print("reached");
			material.Position = material.Position.Snapped(32);
			targetReached = true;
			holder.Velocity = Vector2.Zero;
		}
	}
}