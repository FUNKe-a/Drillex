using drillex.Common.Scripts;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class GameMenu : CanvasLayer
{
	[Export(PropertyHint.File, "*.tscn")] public string MainMenuScene;
	[Export(PropertyHint.File, "*.tres")] public Wallet Wallet { get; set; }
	
	public static TileType SelectedTileType { get; private set; } = 0;
	private bool _isShopOpen = false;
	
	private static VBoxContainer _upgradeMenu; 
	private Button _upgradeButton;
	private Label _upgradeText;
	private Label _priceText;
	private IUpgradable _heldItem;
	private AnimationPlayer _animPlayer;
	private VBoxContainer _tileButtonContainer;
	private Panel _tileShopPanel;
	private Button _OpenShopButton;

	public override void _Ready()
	{
		_upgradeButton = (Button)GetNode("UpgradeMenu/NinePatchRect/Button");
		_upgradeMenu = (VBoxContainer)GetNode("UpgradeMenu");
		_upgradeText = (Label)GetNode("UpgradeMenu/NinePatchRect/Label");
		_priceText = (Label)GetNode("UpgradeMenu/NinePatchRect/PriceTag");
		_upgradeMenu.Visible = false;
		_animPlayer = (AnimationPlayer)GetNode("UpgradeMenu/AnimationPlayer");
		Wallet.MoneyChanged += OnWalletMoneyChanged;
		_tileButtonContainer = (VBoxContainer)GetNode("TileShopPanel/ScrollContainer/TileButtonContainer");
		_tileShopPanel = GetNode<Panel>("TileShopPanel");
		_OpenShopButton = (Button)GetNode("TopMenu/OpenShopButton");
		_OpenShopButton.Pressed += OnOpenShopButtonPressed;
		
		foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
		{
			if (tileType == TileType.NotSelected)
				continue;

			Button tileButton = new Button();

			int price = _tilePrices.TryGetValue(tileType, out int value) ? value : 0;

			tileButton.Text = $"{tileType} (${price}\u20bf)";
			tileButton.Name = tileType.ToString();

			string iconPath = $"res://Assets/Icons/{tileType}.png";
			if (ResourceLoader.Exists(iconPath))
			{
				var iconTexture = GD.Load<Texture2D>(iconPath);
				tileButton.Icon = iconTexture;
			}
			else
			{
				GD.PrintErr($"Icon not found for tile type: {tileType}");
			}

			tileButton.Pressed += () => OnTileButtonPressed(tileType);
			_tileButtonContainer.AddChild(tileButton);
		}
	}
	
	private void OnTileButtonPressed(TileType tileType)
	{
		SelectedTileType = tileType;
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile(MainMenuScene);
	}

	private void OnOpenShopButtonPressed()
	{
		if (_isShopOpen)
		{
			HideTileSelectionMenu();
		}
		else
		{
			ShowTileSelectionMenu();
		}
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
	
	private void ShowTileSelectionMenu()
	{
		var tween = CreateTween();
		Vector2 start = new Vector2(-_tileShopPanel.Size.X, _tileShopPanel.Position.Y);
		Vector2 end = new Vector2(20, _tileShopPanel.Position.Y);

		_tileShopPanel.Position = start;
		tween.TweenProperty(_tileShopPanel, "position", end, 0.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quad);
		_isShopOpen = true;
	}

	private void HideTileSelectionMenu()
	{
		var tween = CreateTween();
		Vector2 offscreen = new Vector2(-_tileShopPanel.Size.X, _tileShopPanel.Position.Y);

		tween.TweenProperty(_tileShopPanel, "position", offscreen, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Quad);
		_isShopOpen = false;
	}
	
	//temp
	private readonly Dictionary<TileType, int> _tilePrices = new()
	{
		{ TileType.Conveyor, 20 },
		{ TileType.Dropper, 60 },
		{ TileType.Furnace, 60 },
		{ TileType.Upgrader, 50 },
	};
}
