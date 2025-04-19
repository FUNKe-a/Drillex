using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class GameMenu : CanvasLayer
{
    [Export(PropertyHint.File, "*.tscn")] public string LocationUponMainMenuButtonPress;
	
	private UpgradeMenu _upgradeMenu;
	private ShopPanel _shopPanel;
	
	public override void _Ready()
	{
		var toolbar = GetNode<Toolbar>("Toolbar");
		_upgradeMenu = GetNode<UpgradeMenu>("UpgradeMenu");
		_shopPanel = GetNode<ShopPanel>("ShopPanel");
		
		toolbar.ConnectToOpenShopButton(_shopPanel.OnOpenShopButtonPressed);
		toolbar.ConnectToMainMenuButton(() => GetTree().ChangeSceneToFile(LocationUponMainMenuButtonPress));
	}

	public void ConnectToTileButtonSelection(Action<TileType> func) =>
		_shopPanel.UpdateAllTileButtonEvents(func);

	public void UpgradeItemScreen<T>(T item) where T : IUpgradable =>
		_upgradeMenu.UpdateUpgradeItemScreen(item);
}