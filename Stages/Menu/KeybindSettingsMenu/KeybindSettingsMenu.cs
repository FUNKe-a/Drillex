using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class KeybindSettingsMenu : VBoxContainer
{
    private GridContainer _grid;

    private bool   _capturing      = false;
    private string _pendingAction  = "";
    private Button _pendingButton  = null;
    
    private Timer _cooldownTimer;
    private bool _isOnCooldown;
    
    private static readonly string[] _actions =
    {
        "Rotate",
        "Place",
        "Delete",
        "Upgrade",
        "CameraPan"
    };

    private static readonly Dictionary<string, List<InputEvent>> _defaultEvents = new();

    private readonly Dictionary<string, Button> _actionButtons = new();

    public override void _Ready()
    {
        _cooldownTimer = new Timer();
        AddChild(_cooldownTimer);
        _cooldownTimer.WaitTime = 1.5;
        _cooldownTimer.Timeout += () => _isOnCooldown = !_isOnCooldown;
            
        _grid = GetNode<GridContainer>("Panel/GridContainer");

        if (_defaultEvents.Count == 0)
        {
            foreach (var action in _actions)
            {
                _defaultEvents[action] = InputMap.ActionGetEvents(action)
                    .Select(e => (InputEvent)e.Duplicate())
                    .ToList();
            }
        }

        foreach (var action in _actions)
        {
            var label  = new Label  { Text = action };
            var button = new Button { Text = FirstEventText(action) };

            button.Pressed += () =>
            {
                if (!_isOnCooldown) 
                    BeginCapture(action, button);
            };

            _grid.AddChild(label);
            _grid.AddChild(button);

            _actionButtons[action] = button;
        }
    }

    private void BeginCapture(string action, Button btn)
    {
        _capturing     = true;
        _pendingAction = action;
        _pendingButton = btn;
        btn.Text       = "Press a key…";
    }

    public override void _Input(InputEvent ev)
    {
        if (_isOnCooldown) return;
        if (!_capturing) return;

        if (ev is InputEventMouseMotion || ev is InputEventJoypadMotion) return;
        if (!ev.IsPressed()) return;

        if (IsEventAlreadyBound(ev, _pendingAction))
        {
            _pendingButton.Text = $"{ev.AsText()} already bound!";
            _capturing          = false;
            _pendingAction      = "";
            _pendingButton      = null;

            _isOnCooldown = true;
            _cooldownTimer.Start();
            return;
        }

        InputMap.ActionEraseEvents(_pendingAction);
        InputMap.ActionAddEvent   (_pendingAction, ev);

        _pendingButton.Text = ev.AsText();
        _capturing          = false;
        _pendingAction      = "";
        _pendingButton      = null;
    }

    private static bool IsEventAlreadyBound(InputEvent ev, string currentAction)
    {
        foreach (var action in _actions)
        {
            if (action == currentAction) continue;
            if (InputMap.ActionHasEvent(action, ev))
                return true;
        }
        return false;
    }

    private static string FirstEventText(string action)
    {
        var events = InputMap.ActionGetEvents(action);
        return events.Count > 0 ? events[0].AsText() : "Unbound";
    }

    private void OnBackButtonUp() =>
        QueueFree();

    private void OnResetButtonUp()
    {
        foreach (var action in _actions)
        {
            InputMap.ActionEraseEvents(action);
            foreach (var ev in _defaultEvents[action])
                InputMap.ActionAddEvent(action, (InputEvent) ev.Duplicate());

            _actionButtons[action].Text = FirstEventText(action);
        }
    }
}