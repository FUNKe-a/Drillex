using Godot;
using System;
using Godot.Collections;

namespace drillex.Assets.Entities.Furnace;

public partial class Furnace : TileMapLayer
{
	Node2D _materialHolder;
	Array<Vector2I> _furnaceDirection;
	
	//the key is the block in front of the furnace, and the value is the location of the furnace
	Dictionary<Vector2I, Vector2I> _furnaces;

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
		foreach (var node in _materialHolder.GetChildren())
		{
			Material material = (Material)node;
			
			
		}
	}


	public void AddFurnace(Vector2I mapPosition, Vector2I dropperAtlasPosition)
	{
		SetCell(mapPosition, 0, dropperAtlasPosition);
	}

	public void RemoveFurnace(Vector2I mapPosition)
	{
		EraseCell(mapPosition);
	}
}