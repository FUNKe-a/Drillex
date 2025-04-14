using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections;

public partial class GameMenu : CanvasLayer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	[Export(PropertyHint.File, "*.tres")] public Wallet Wallet { get; set; }
	
	public static TileType SelectedTileType { get; private set; } = 0;
	
	private static VBoxContainer _upgradeMenu; 
	private OptionButton  _tileSelectionButton;
	private Button _upgradeButton;
	private Label _upgradeText;
	private Label _priceText;
	private IUpgradable _heldItem;
	private AnimationPlayer _animPlayer;

	public override void _Ready()
	{
		_tileSelectionButton = (OptionButton)GetNode("TopMenu/TileSelectionButton");
		_upgradeButton = (Button)GetNode("UpgradeMenu/NinePatchRect/Button");
		_upgradeMenu = (VBoxContainer)GetNode("UpgradeMenu");
		_upgradeText = (Label)GetNode("UpgradeMenu/NinePatchRect/Label");
		_priceText = (Label)GetNode("UpgradeMenu/NinePatchRect/PriceTag");
		_upgradeMenu.Visible = false;
		_animPlayer = (AnimationPlayer)GetNode("UpgradeMenu/AnimationPlayer");
		Wallet.MoneyChanged += OnWalletMoneyChanged;
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile(MainMenuScene);
	}
	
	private void OnTileSelectionButtonItemSelected(int index)
	{
		SelectedTileType = (TileType)index;
	}
	
	public void UpgradeItemScreen<T>(T item) where T : IUpgradable{
		if (item.SufficientFunds())
			_priceText.AddThemeColorOverride("font_color", new Color("00ff00"));
		else 
			_priceText.AddThemeColorOverride("font_color", new Color("ff0000"));
		
		_priceText.Text = item.GetPrice();
		
		if(!_upgradeMenu.Visible) 
			_animPlayer.Play("OpenUpgradeMenu");
		
		_heldItem = item;
		_upgradeText.Text = item.UpgradeText();
	}

	private void OnWalletMoneyChanged()
	{
		if(_upgradeMenu.Visible) 
			UpgradeItemScreen(_heldItem);
	}

	private void UpgradeButtonPressed()
	{
		if(_heldItem.SufficientFunds()) 
		{
			_heldItem.Upgrade();
			UpgradeItemScreen(_heldItem);
		}
	}
	
	private void CloseUpgradePressed(){
		_animPlayer.Play("CloseUpgradeMenu");
	}
}
