using Godot;

namespace MyGame.Scripts;

public partial class Brick : StaticBody2D
{
    public float Width =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;

    public override void _Ready()
    {
        base._Ready();

        GD.Print("Brick Width " + Width);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
