using Godot;
using System;

public partial class MoneyLabel : Label
{
	[Export] public Wallet WalletResource { get; set; }

	public override void _Ready()
	{
		if (WalletResource != null)
		{
			OnMoneyChanged();
			WalletResource.MoneyChanged += OnMoneyChanged; 
		}
	}
	
	private void OnMoneyChanged()
	{
		Text = $"Money: ${WalletResource.Money}";
	}

	public override void _ExitTree()
	{
		WalletResource.MoneyChanged -= OnMoneyChanged;
	}
}
