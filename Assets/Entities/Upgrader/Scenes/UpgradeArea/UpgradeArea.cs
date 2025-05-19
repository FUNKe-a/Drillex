using Godot;
using System.Collections.Generic;
using drillex.Common.Scripts;

public partial class UpgradeArea : Area2D, IUpgradable
{
    [Export] public float IncreaseMultiplier { get; private set; }
    [Export] public int PassAmount { get; private set; }
    [Export] public int UpgradeCeiling { get; private set; }

    private Dictionary<ulong, int> _upgradeCounts;
    private int _level;
    private AnimatedSprite2D _sprite;

    private ulong _upgradePrice;
    private string _animation;

    public ulong UpgradePrice
    {
        get => _upgradePrice;
        private set => _upgradePrice = value;
    }

    public override void _Ready()
    {
        _level = 1;
        UpgradePrice = 500;
        _upgradeCounts = new Dictionary<ulong, int>();
        _sprite = GetNode<AnimatedSprite2D>("Sprite2D");
        _sprite.AnimationFinished += () =>
        {
            if (_sprite.GetAnimation() != "Idle")
                _sprite.Play("Idle");
        };
        _animation = "Refine1";
    }

    public bool Upgrade()
    {
        if (_level < 3)
        {
            _level++;
            switch (_level)
            {
                case 2:
                    UpgradeCeiling += 1;
                    UpgradePrice = 1000;
                    _animation = "Refine2";
                    break;
                case 3:
                    UpgradeCeiling += 1;
                    UpgradePrice = 0;
                    _animation = "Refine3";
                    break;
            }

            return true;
        }

        return false;
    }

    public string UpgradeText() =>
        $"Type: Upgrader\nUpgrade ceiling: {UpgradeCeiling}\nLevel: {_level}";

    public string GetPriceText() =>
        _level == 3 ? "level MAX" : $"Price: {UpgradePrice}\u20bf";

    private void OnAreaEntered(Area2D body)
    {
        if (body is Material mat)
        {
            _sprite.Play(_animation);
            ulong oreId = mat.GetInstanceId();

            if (_upgradeCounts.TryAdd(oreId, 0))
                mat.TreeExited += () => OnMaterialExited(oreId);

            if (_upgradeCounts[oreId] < PassAmount && mat.UpgradeCount < UpgradeCeiling)
            {
                mat.Multiplier += IncreaseMultiplier;
                _upgradeCounts[oreId]++;
                mat.UpgradeCount++;
            }
        }
    }

    private void OnMaterialExited(ulong oreId) =>
        _upgradeCounts.Remove(oreId);
}