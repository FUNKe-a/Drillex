using drillex.Common.Scripts;
using Godot;
using System;


public partial class UpgradeMenu : VBoxContainer
{
	[Export(PropertyHint.File, "*.tres")] public Wallet Wallet { get; set; }

	private static VBoxContainer _upgradeMenu;
	private Button _upgradeButton;
	private Label _upgradeText;
	private Label _priceText;
	private IUpgradable _heldItem;
	private AnimationPlayer _animPlayer;

	
	public override void _Ready()
	{
		_upgradeButton = (Button)GetNode("NinePatchRect/Button");
		_upgradeMenu = (VBoxContainer)GetNode(".");
		_upgradeText = (Label)GetNode("NinePatchRect/Label");
		_priceText = (Label)GetNode("NinePatchRect/PriceTag");
		_upgradeMenu.Visible = false;
		_animPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
		Wallet.MoneyChanged += OnWalletMoneyChanged;
	}

	public void UpgradeItemScreen<T>(T item) where T : IUpgradable
	{
		if (item.SufficientFunds())
			_priceText.AddThemeColorOverride("font_color", new Color("00ff00"));
		else
			_priceText.AddThemeColorOverride("font_color", new Color("ff0000"));

		_priceText.Text = item.GetPrice();

		if (!_upgradeMenu.Visible)
			_animPlayer.Play("OpenUpgradeMenu");

		_heldItem = item;
		_upgradeText.Text = item.UpgradeText();
	}

	private void OnWalletMoneyChanged()
	{
		if (_upgradeMenu.Visible)
			UpgradeItemScreen(_heldItem);
	}

	private void UpgradeButtonPressed()
	{
		if (_heldItem.SufficientFunds())
		{
			_heldItem.Upgrade();
			UpgradeItemScreen(_heldItem);
		}
	}

	private void CloseUpgradePressed()
	{
		_animPlayer.Play("CloseUpgradeMenu");
	}

}
