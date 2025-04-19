using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class GameMenu : CanvasLayer
{
    [Export(PropertyHint.File, "*.tscn")] public string LocationUponPress;
	public static TileType SelectedTileType { get; private set; } = 0;
	
	private UpgradeMenu _upgradeMenu;
	private ShopPanel _shopPanel;
	private Toolbar _toolbar;
	
	public override void _Ready()
	{
		_toolbar = GetNode<Toolbar>("Toolbar");
		_upgradeMenu = GetNode<UpgradeMenu>("UpgradeMenu");
		_shopPanel = GetNode<ShopPanel>("ShopPanel");
		
		_toolbar.ConnectToOpenShopButton(_shopPanel.OnOpenShopButtonPressed);
		_toolbar.ConnectToMainMenuButton(() => GetTree().ChangeSceneToFile(LocationUponPress));
	}
	
	private void OnTileButtonPressed(TileType tileType)
	{
		SelectedTileType = tileType;
	}

	public void UpgradeItemScreen<T>(T item) where T : IUpgradable
	{
		_upgradeMenu.UpdateUpgradeItemScreen(item);
	}
}
