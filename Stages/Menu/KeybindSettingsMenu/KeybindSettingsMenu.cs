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

    private static readonly string[] _actions =
    {
        "Rotate",
        "Place",
        "Delete",
        "Upgrade",
    };

    private static readonly Dictionary<string, List<InputEvent>> _defaultEvents = new();

    private readonly Dictionary<string, Button> _actionButtons = new();

    public override void _Ready()
    {
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

            button.Pressed += () => BeginCapture(action, button);

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
        if (!_capturing) return;

        if (ev is InputEventMouseMotion || ev is InputEventJoypadMotion) return;
        if (!ev.IsPressed()) return;

        InputMap.ActionEraseEvents(_pendingAction);
        InputMap.ActionAddEvent   (_pendingAction, ev);

        _pendingButton.Text = ev.AsText();
        _capturing          = false;
        _pendingAction      = "";
        _pendingButton      = null;
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
