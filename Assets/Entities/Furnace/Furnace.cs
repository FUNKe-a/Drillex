using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace drillex.Assets.Entities.Furnace;

public partial class Furnace : TileMapLayer
{
	[Export] private Wallet _wallet;
	
	Node2D _materialHolder;
	
	//Location of furnaces
	HashSet<Vector2> _furnaces;

	public override void _Ready()
	{
		_materialHolder = GetNode<Node2D>("../MaterialHolder");
		_furnaces = new HashSet<Vector2>();
	}

	public override void _PhysicsProcess(double delta)
	{
		foreach (var node in _materialHolder.GetChildren())
		{
			Material material = (Material)node;

			if (_furnaces.Contains(material.Position))
			{
				material.QueueFree();
				_wallet.AddMoney(material.MonetaryValue);
			}
		}
	}


	public void AddFurnace(Vector2I mapPosition)
	{
		SetCell(mapPosition, 0, new Vector2I(0, 0));
		_furnaces.Add(MapToLocal(mapPosition) - new Vector2I(16, 16));
	}

	public void RemoveFurnace(Vector2I mapPosition)
	{
		EraseCell(mapPosition);
		_furnaces.Remove(MapToLocal(mapPosition) - new Vector2I(16, 16));
	}
}