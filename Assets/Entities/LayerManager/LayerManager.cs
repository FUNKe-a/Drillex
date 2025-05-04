using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using drillex.Common.Scripts;
using Godot;
using Range = Godot.Range;

namespace drillex.Assets.Entities.LayerManager;

public partial class LayerManager : Node2D
{
	[Signal]
	public delegate void RotationChangedEventHandler(int rotationID);
	
	[Export] public int XBoundary;
	[Export] public int YBoundary;
	[Export] public Wallet WalletResource { get; set; }

	BackgroundTile [,] _occupiedPositions;
	Conveyor.Conveyor _conveyorLayer; 
	Dropper.Dropper _dropperLayer;
	Furnace.Furnace _furnaceLayer;
	Upgrader.Upgrader _upgraderLayer;

	private int _rotationID;
	string _action;

	private GameMenu _gameMenu;
	private TileType _selectedTileType;

	public override void _Ready()
	{
		if (XBoundary == 0)
			XBoundary = (int)Math.Ceiling(GetViewport().GetVisibleRect().Size.X / 32f);
		if (YBoundary == 0)
			YBoundary = (int)Math.Ceiling(GetViewport().GetVisibleRect().Size.Y / 32f);

		_occupiedPositions = GetNode<Background>("Background").CreateBackgroundMatrix(XBoundary, YBoundary);

		_gameMenu = GetNode<GameMenu>("../GameMenu");
		_conveyorLayer = GetNode<Conveyor.Conveyor>("Conveyor");
		_dropperLayer = GetNode<Dropper.Dropper>("Dropper");
		_furnaceLayer = GetNode<Furnace.Furnace>("Furnace");
		_upgraderLayer = GetNode<Upgrader.Upgrader>("Upgrade");
		_rotationID = 0;
		_action = "Idle";
		
		_gameMenu.ConnectToTileButtonSelection(UpdateSelectedTileType);
		WalletResource.ResetMoney();
	}

	public override void _Process(double delta)
	{
		switch (_action)
		{
			case "Place" :
				AddTile(_selectedTileType);
				break;
			case "Delete" :
				RemoveTile();
				break;
			case "Upgrade":
				UpgradeTile();
				break;
			case "Idle" :
				return;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Place"))
			_action = "Place";
		if (@event.IsActionReleased("Place"))
			_action = "Idle";
		if (@event.IsActionPressed("Delete"))
			_action = "Delete";
		if (@event.IsActionReleased("Delete"))
			_action = "Idle";
		if (@event.IsActionPressed("Upgrade"))
			_action = "Upgrade";
		if (@event.IsActionReleased("Upgrade"))
			_action = "Idle";
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionReleased("Rotate"))
		{
			_rotationID = TileRotation(_rotationID, 1);
			EmitSignal(SignalName.RotationChanged, _rotationID);
		}
	}
	
	public void UpgradeTile(){
		Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
		var item = _occupiedPositions[mapPosition.X, mapPosition.Y].TileType;
		
		if (item is not TileType.NotSelected &&
			mapPosition.X < XBoundary && mapPosition.X >= 0 &&
			mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
			_occupiedPositions[mapPosition.X, mapPosition.Y].IsOccupied) {
			switch (item)
			{
				case TileType.Conveyor:
					break;
				case TileType.Dropper:
					_gameMenu.UpgradeItemScreen(_dropperLayer.GetDropper(mapPosition));
					break;
				case TileType.Furnace:
					break;
				case TileType.Upgrader:
					break;
			}
		}
	}

	public void AddTile(TileType tileType)
	{
		Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
		
		if (tileType is not TileType.NotSelected &&
			mapPosition.X < XBoundary && mapPosition.X >= 0 && 
			mapPosition.Y < YBoundary && mapPosition.Y >= 0 && 
			!_occupiedPositions[mapPosition.X, mapPosition.Y].IsOccupied)
		{
			bool isTileBought = false;
			
			switch (tileType)
			{
				case TileType.Conveyor :
					if (WalletResource.TrySpend(20))
					{
						_conveyorLayer.AddConveyor(mapPosition, _rotationID);
						_occupiedPositions[mapPosition.X, mapPosition.Y].TileType = TileType.Conveyor;
						isTileBought = true;
					}
					break;
				case TileType.Dropper :
					if (WalletResource.TrySpend(60))
					{
						_dropperLayer.AddDropper(
							mapPosition, 
							_rotationID, 
							_occupiedPositions[mapPosition.X, mapPosition.Y].IsMineable);
						_occupiedPositions[mapPosition.X, mapPosition.Y].TileType = TileType.Dropper;
						isTileBought = true;
					}
					break;
				case TileType.Furnace :
					if (WalletResource.TrySpend(60))
					{
						_conveyorLayer.AddConveyor(mapPosition, 0, true);
						_furnaceLayer.AddFurnace(mapPosition);
						_occupiedPositions[mapPosition.X, mapPosition.Y].TileType = TileType.Furnace;
						isTileBought = true;
					}
					break;
				case TileType.Upgrader :
					if (WalletResource.TrySpend(50))
					{
						_conveyorLayer.AddConveyor(mapPosition, _rotationID);
						_upgraderLayer.AddUpgrader(mapPosition, _rotationID);
						_occupiedPositions[mapPosition.X, mapPosition.Y].TileType = TileType.Upgrader;
						isTileBought = true;
					}
					break;
				default :
					return;
			}
			if (isTileBought) 
				_occupiedPositions[mapPosition.X, mapPosition.Y].IsOccupied = true;
		}
	}

	public void RemoveTile()
	{
		Vector2I mapPosition = _conveyorLayer.LocalToMap(GetLocalMousePosition());
		
		if (mapPosition.X < XBoundary && mapPosition.X >= 0 && 
			mapPosition.Y < YBoundary && mapPosition.Y >= 0 &&
			_occupiedPositions[mapPosition.X, mapPosition.Y].IsOccupied)
		{
			switch (_occupiedPositions[mapPosition.X, mapPosition.Y].TileType)
			{
				case TileType.Conveyor:
					_conveyorLayer.RemoveConveyor(mapPosition);
					WalletResource.AddMoney(10);
					break;
				case TileType.Dropper:
					_dropperLayer.RemoveDropper(mapPosition);
					WalletResource.AddMoney(30);
					break;
				case TileType.Furnace:
					_furnaceLayer.RemoveFurnace(mapPosition);
					_conveyorLayer.RemoveConveyor(mapPosition);
					WalletResource.AddMoney(30);
					break;
				case TileType.Upgrader :
					_upgraderLayer.RemoveUpgrader(mapPosition);
					_conveyorLayer.RemoveConveyor(mapPosition);
					WalletResource.AddMoney(20);
					break;
			}
			_occupiedPositions[mapPosition.X, mapPosition.Y].TileType = TileType.NotSelected;
			_occupiedPositions[mapPosition.X, mapPosition.Y].IsOccupied = false;
		}
	}

	private int TileRotation(int rotationID, int value)
	{
		int returnID = rotationID;
		
		for (int i = 0; i < value; i++)
		{
			returnID++;
			if (returnID == 4)
				returnID = 0;
		}

		return returnID;
	}

	private void UpdateSelectedTileType(TileType tileType) =>
		_selectedTileType = tileType;
}
