using Godot;

namespace drillex.Assets.Entities.Conveyor;

public partial class MaterialMovementHolder : GodotObject
{
    public Vector2 Velocity { get; set; }
    public Vector2I TargetPosition { get; set; }

    public MaterialMovementHolder()
    {
        Velocity = Vector2.Zero;
        TargetPosition = Vector2I.Zero;
    }
}