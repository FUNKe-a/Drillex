using Godot;
using System;

[GlobalClass]
public partial class Wallet : Resource
{
	[Signal]
	public delegate void MoneyChangedEventHandler();

	private ulong _money;

	[Export]
	public ulong Money
	{
		get => _money;
		set
		{
			if (_money == value) return;
			_money = value;
			EmitSignal(SignalName.MoneyChanged);
		}
	}
}
