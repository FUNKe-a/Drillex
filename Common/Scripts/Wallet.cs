using Godot;
using System;

[GlobalClass]
public partial class Wallet : Resource
{
    [Export] public int Money { get; set; }
}
