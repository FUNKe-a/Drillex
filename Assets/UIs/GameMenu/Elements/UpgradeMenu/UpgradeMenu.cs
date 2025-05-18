using drillex.Common.Scripts;
using Godot;
using System;
using System.Reflection;


public partial class UpgradeMenu : VBoxContainer
{
	private Button _upgradeButton;
	private Label _upgradeText;
	private Label _priceText;
	private AnimationPlayer _animPlayer;
	private Vector2 _startingPosition;

	public bool IsShopOpen { get; private set; }

	public override void _Ready()
	{
		_upgradeButton = GetNode<Button>("TextureRect/Button");
		_upgradeText = GetNode<Label>("TextureRect/Label");
		_priceText = GetNode<Label>("TextureRect/PriceTag");
		_startingPosition = Position;
	}

	public void UpdateMenuData<T>(T item) where T : IUpgradable
	{
		_priceText.Text = item.GetPriceText();
		_upgradeText.Text = item.UpgradeText();
	}

	public void ChangePriceFontColor(Color color) =>
		_priceText.AddThemeColorOverride("font_color", color);

	public void ConnectToUpgradeButtonPressed(Action uponButtonPress) =>
		GetNode<UpgradeButton>("TextureRect/Button").ConnectToUpgradeButtonPressed(uponButtonPress);

	private void CloseUpgradePressed()
	{
		var tween = CreateTween();
		Vector2 end = _startingPosition;

		tween.TweenProperty(this, "position", end, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Quad);
		IsShopOpen = false;
	}

	public void ShowUpgradeMenu()
	{
		var tween = CreateTween();
		Vector2 end = _startingPosition - new Vector2(Size.X + 48, 0);

		tween.TweenProperty(this, "position", end, 0.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quad);
		IsShopOpen = true;
	}
}
