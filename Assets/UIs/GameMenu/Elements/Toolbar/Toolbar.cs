using Godot;
using System;

public partial class Toolbar : HBoxContainer
{
	public void ConnectToOpenShopButton(Action func) =>
		GetNode<Button>("OpenShopButton").ButtonUp += func;

	public void ConnectToMainMenuButton(Action func) =>
		GetNode<Button>("MainMenuButton").ButtonUp += func;
}
