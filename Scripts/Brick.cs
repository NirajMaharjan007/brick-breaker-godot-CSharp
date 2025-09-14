using Godot;

namespace MyGame.Scripts;

public partial class Brick : StaticBody2D
{
    public float Width =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;

    public float Height =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;

    public override void _Ready()
    {
        base._Ready();

        GD.Print($"Brick Width {Width} and height {Height}");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
