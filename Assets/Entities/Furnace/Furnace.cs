using Godot;
using System;
using Godot.Collections;

namespace drillex.Assets.Entities.Furnace;

public partial class Furnace : TileMapLayer
{
	Node2D _materialHolder;
	Array<Vector2I> _furnaceDirection;
	Dictionary<Vector2I, MaterialMovementHolder> _materialMovementHolders;
	
	public override void _Ready()
	{
		_materialHolder = GetNode<Node2D>("../MaterialHolder");	
		_furnaceDirection = new Array<Vector2I>();
		
		_furnaceDirection.Add(Vector2I.Up);
		_furnaceDirection.Add(Vector2I.Right);
		_furnaceDirection.Add(Vector2I.Down);
		_furnaceDirection.Add(Vector2I.Left);
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
