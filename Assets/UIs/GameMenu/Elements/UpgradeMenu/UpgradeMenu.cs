using drillex.Common.Scripts;
using Godot;
using System;


public partial class UpgradeMenu : VBoxContainer
{
	private static VBoxContainer _upgradeMenu;
	private Button _upgradeButton;
	private Label _upgradeText;
	private Label _priceText;
	private AnimationPlayer _animPlayer;
	
	public override void _Ready()
	{
		_upgradeButton = GetNode<Button>("TextureRect/Button");
		_upgradeMenu = GetNode<VBoxContainer>(".");
		_upgradeText = GetNode<Label>("TextureRect/Label");
		_priceText = GetNode<Label>("TextureRect/PriceTag");
		_upgradeMenu.Visible = false;
		_animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void UpdateMenuData<T>(T item) where T : IUpgradable
	{
		_priceText.Text = item.GetPriceText();
		_upgradeText.Text = item.UpgradeText();
	}

	public void ChangePriceFontColor(Color color) =>
		_priceText.AddThemeColorOverride("font_color", color);

	public void ConnectToUpgradeButtonPressed(Action uponButtonPress) =>
		GetNode<Button>("TextureRect/Button").Pressed += uponButtonPress;

	private void CloseUpgradePressed() =>
		_animPlayer.Play("CloseUpgradeMenu");

	public void ShowUpgradeMenu() =>
			_animPlayer.Play("OpenUpgradeMenu");
}
