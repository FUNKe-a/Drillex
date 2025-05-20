using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ShopPanel : Panel
{
	private bool _isShopOpen;
	private Panel _tileShopPanel;
	private VBoxContainer _tileButtonContainer;
	private Vector2 _startingPosition;
	
	//temp
	private readonly Dictionary<TileType, int> _tilePrices = new()
	{
		{ TileType.Conveyor, 20 },
		{ TileType.MiningRig, 60 },
		{ TileType.Furnace, 60 },
		{ TileType.Refiner, 50 },
	};
	
	public override void _Ready()
	{
		_tileButtonContainer = GetNode<VBoxContainer>("ScrollContainer/TileButtonContainer");
		foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
		{
			if (tileType == TileType.NotSelected)
				continue;

			Button tileButton = new Button();

			int price = _tilePrices.GetValueOrDefault(tileType, 0);

			var name = SplitCamelCase(tileType.ToString());
			tileButton.Text = $"{name} (${price})";
			tileButton.Name = tileType.ToString();

			string iconPath = $"res://Assets/Icons/{tileType}.png";
			if (ResourceLoader.Exists(iconPath))
			{
				var iconTexture = GD.Load<Texture2D>(iconPath);
				tileButton.Icon = iconTexture;
			}
			else GD.PrintErr($"Icon not found for tile type: {tileType}");

			_tileButtonContainer.AddChild(tileButton);
			_startingPosition = Position;
		}
	}

	private string SplitCamelCase(string input)
	{
		string separatedString = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
		string[] words = separatedString.Split(' ');

		for (int i = 1; i < words.Length; i++)
			words[i] = words[i].ToLower();
		
		return string.Join(" ", words);
	}

	public void OnOpenShopButtonPressed()
    {
    	if (_isShopOpen) HideTileSelectionMenu();
    	else ShowTileSelectionMenu();
    }

	public void UpdateAllTileButtonEvents(Action<TileType> func)
	{
		foreach (var node in _tileButtonContainer.GetChildren())
		{
			var button = (Button)node;
			TileType value = (TileType)Enum.Parse(typeof(TileType), node.Name);
			button.Pressed += () => func(value);
		}
	}
		
	private void ShowTileSelectionMenu()
	{
		var tween = CreateTween();
		Vector2 end = _startingPosition + new Vector2(Size.X + 48, 0);

		tween.TweenProperty(this, "position", end, 0.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quad);
		_isShopOpen = true;
	}
	
	private void HideTileSelectionMenu()
	{
		var tween = CreateTween();
		Vector2 end = _startingPosition;

		tween.TweenProperty(this, "position", end, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Quad);
		_isShopOpen = false;
	}
}
