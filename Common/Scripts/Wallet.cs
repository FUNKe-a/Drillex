using Godot;
using System;

[GlobalClass]
public partial class Wallet : Resource
{
    [Signal]
    public delegate void MoneyChangedEventHandler();

    [Export]
    public ulong Money { get; private set; }
	
    public bool HasEnough(ulong cost) => Money >= cost;

    public bool TrySpend(ulong cost)
    {
        if (Money >= cost)
        {
            Money -= cost;
            EmitSignal(SignalName.MoneyChanged);
            return true;
        }
        return false;
    }

    public void AddMoney(ulong amount)
    {
        Money += amount;
        EmitSignal(SignalName.MoneyChanged);
    }

    public void ResetMoney()
    {
        Money = 200;
        EmitSignalMoneyChanged();
    }
}