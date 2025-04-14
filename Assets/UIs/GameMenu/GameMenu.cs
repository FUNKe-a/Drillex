using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections;

public partial class GameMenu : CanvasLayer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;

	public static TileType SelectedTileType { get; private set; } = 0;
	
	private OptionButton  _tileSelectionButton;
	private UpgradeMenu _upgradeMenu;

	public override void _Ready()
	{
		_tileSelectionButton = (OptionButton)GetNode("TopMenu/TileSelectionButton");
		_upgradeMenu = (UpgradeMenu)GetNode("UpgradeMenu");
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile(MainMenuScene);
	}
	
	private void OnTileSelectionButtonItemSelected(int index)
	{
		SelectedTileType = (TileType)index;
	}

	public void UpgradeItemScreen<T>(T item) where T : IUpgradable
	{
		_upgradeMenu.UpgradeItemScreen(item);
	}


}
