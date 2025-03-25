using Godot;
using System;

public partial class Wallet : Resource
{
    public static int Money { get; set; }

    public Wallet()
    {
        Money = 0;
    }
}
