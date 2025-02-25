using Godot;
using Godot.Collections;

public partial class Conveyor : TileMapLayer
{
	[Export]
	public float Speed { get; set; }
	[Export] 
	public Node2D Holder { get; set; }

	private Array<Vector2> _conveyorVelocity = new Array<Vector2>();
	private Array<Vector2I> _conveyorDirection = new Array<Vector2I>();
	
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
			Vector2 materialVelocity = GetMaterialVelocity(material);
			material.Position += materialVelocity * (float)d;
		}
	}

	private Vector2 GetMaterialVelocity(Material material)
	{
    	var materialPosition = ToLocal(material.GlobalPosition);
    	var mapPosition = LocalToMap(materialPosition);
    	
    	int directionIndex = GetCellAtlasCoords(mapPosition).X;
    	if (directionIndex != -1)
    	{
    		var conveyorVelocity = _conveyorVelocity[directionIndex];
    		var conveyorDirection = _conveyorDirection[directionIndex];
    		
     		int frontDirectionIndex = GetCellAtlasCoords(mapPosition + -conveyorDirection).X;
     		
         	if (material.PreviousDirection != Vector2.Zero && conveyorDirection != material.PreviousDirection)
         		material.Position = MapToLocal(mapPosition) + new Vector2(-16, -16);
         	
         	/*material.Position += conveyorVelocity * (float)d;*/
         	material.PreviousDirection = _conveyorDirection[directionIndex];			
		    return conveyorVelocity;
	    }
	    return Vector2.Zero;
	}
}