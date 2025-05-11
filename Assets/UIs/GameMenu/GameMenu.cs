using Godot;
using System;
using drillex.Assets.Entities.LayerManager;

public partial class GameMenu : CanvasLayer
{
    [Export(PropertyHint.File, "*.tscn")] 
    public string LocationUponMainMenuButtonPress;
    [Export] 
    public LayerManager LayerManager;
	
	private UpgradeMenu _upgradeMenu;
	private ShopPanel _shopPanel;
	
	public override void _Ready()
	{
		var toolbar = GetNode<Toolbar>("Toolbar");
		_upgradeMenu = GetNode<UpgradeMenu>("UpgradeMenu");
		_shopPanel = GetNode<ShopPanel>("ShopPanel");
		GetNode<DirectionArrow>("Toolbar/DirectionArrow").LayerManager = LayerManager;
		
		toolbar.ConnectToOpenShopButton(_shopPanel.OnOpenShopButtonPressed);
		toolbar.ConnectToMainMenuButton(() => GetTree().ChangeSceneToFile(LocationUponMainMenuButtonPress));
	}

	public void ConnectToTileButtonSelection(Action<TileType> func) =>
		_shopPanel.UpdateAllTileButtonEvents(func);

	public UpgradeMenu GetUpgradeMenu() =>
		GetNode<UpgradeMenu>("UpgradeMenu");
}