using Godot;
using System;

[GlobalClass]
public partial class Wallet : Resource
{
    public Action MoneyChanged;

    [Export] 
    public ulong StartingMoney;
    
    public ulong Money { get; private set; }
	
    public bool HasEnough(ulong cost) => Money >= cost;

    public bool TrySpend(ulong cost)
    {
        if (Money >= cost)
        {
            Money -= cost;
            MoneyChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void AddMoney(ulong amount)
    {
        Money += amount;
        MoneyChanged?.Invoke();
    }

    public void ResetMoney()
    {
        Money = StartingMoney;
        MoneyChanged?.Invoke();
    }
}