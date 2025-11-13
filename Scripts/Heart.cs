using Godot;

namespace MyGame.Scripts;

public partial class Heart : AnimatedSprite2D
{
    private float width;
    private float height;

    public override void _Ready()
    {
        base._Ready();

        var area = GetNode<Area2D>("Area2D");
        var shape = area.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        width = shape?.Size.X ?? 0f;
        height = shape?.Size.Y ?? 0f;
    }

    public float Width => width;
    public float Height => height;
}
