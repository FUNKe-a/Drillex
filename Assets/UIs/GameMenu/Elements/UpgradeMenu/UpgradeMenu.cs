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
		_upgradeButton = (Button)GetNode("TextureRect/Button");
		_upgradeMenu = (VBoxContainer)GetNode(".");
		_upgradeText = (Label)GetNode("TextureRect/Label");
		_priceText = (Label)GetNode("TextureRect/PriceTag");
		_upgradeMenu.Visible = false;
		_animPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
		Wallet.MoneyChanged += OnWalletMoneyChanged;
	}

	public void UpdateUpgradeItemScreen<T>(T item) where T : IUpgradable
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
			UpdateUpgradeItemScreen(_heldItem);
	}

	private void UpgradeButtonPressed()
	{
		if (_heldItem.SufficientFunds())
		{
			_heldItem.Upgrade();
			UpdateUpgradeItemScreen(_heldItem);
		}
	}

	private void CloseUpgradePressed()
	{
		_animPlayer.Play("CloseUpgradeMenu");
	}

	public override void _ExitTree()
	{
		Wallet.MoneyChanged -= OnWalletMoneyChanged;
	}

}
