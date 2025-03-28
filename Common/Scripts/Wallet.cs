using Godot;

[GlobalClass]
public partial class Wallet : Resource
{
    [Export] public ulong Money { get; set; }
}
