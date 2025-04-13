using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections;

public partial class GameMenu : CanvasLayer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	
	public static TileType SelectedTileType { get; private set; } = 0;
	static VBoxContainer UpgradeMenu; 
	
	private OptionButton  _tileSelectionButton;
	private Button UpgradeButton;
	private Label UpgradeText;
	private Label PriceText;
	private IUpgradable HeldItem;
	private AnimationPlayer AnimPlayer;
	private Wallet Wallet = ResourceLoader.Load<Wallet>("res://Assets/Resources/Wallet.tres");


	public override void _Ready()
	{
		_tileSelectionButton = (OptionButton)GetNode("TopContainer/TileSelectionButton");
		UpgradeButton = (Button)GetNode("UpgradeMenu/NinePatchRect/Button");
		UpgradeMenu = (VBoxContainer)GetNode("UpgradeMenu");
		UpgradeText = (Label)GetNode("UpgradeMenu/NinePatchRect/Label");
		PriceText = (Label)GetNode("UpgradeMenu/NinePatchRect/PriceTag");
		UpgradeMenu.Visible = false;
		AnimPlayer = (AnimationPlayer)GetNode("UpgradeMenu/AnimationPlayer");
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
		{
			PriceText.AddThemeColorOverride("font_color", new Color("00ff00"));

		} else PriceText.AddThemeColorOverride("font_color", new Color("ff0000"));
		PriceText.Text = item.GetPrice();
		if(!UpgradeMenu.Visible) AnimPlayer.Play("OpenUpgradeMenu");
		HeldItem = item;
		UpgradeText.Text = item.UpgradeText();
	}

	private void OnWalletMoneyChanged()
	{
		if(UpgradeMenu.Visible) UpgradeItemScreen(HeldItem);

	}

	private void UpgradeButtonPressed()
	{
		if(HeldItem.SufficientFunds()) {
			HeldItem.Upgrade();
			UpgradeItemScreen(HeldItem);
		}
	}
	
	private void CloseUpgradePressed(){
		AnimPlayer.Play("CloseUpgradeMenu");
	}
}
