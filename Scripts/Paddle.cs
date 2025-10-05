using Godot;

namespace MyGame.Scripts;

public partial class Paddle : StaticBody2D
{
    private const int SPEED = 320;

    private AnimatedSprite2D animated;
    public float Width =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;

    public float Height =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;

    public override void _Ready()
    {
        base._Ready();

        // GD.Print($"WIDTH -> {Width}");
        animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animated.Pause();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Move(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    private void Move(double delta)
    {
        var velocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_left"))
        {
            velocity.X -= 1;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_right"))
        {
            animated.Play();
        }

        if (Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
        {
            animated.Pause();
        }

        Position += velocity.Normalized() * SPEED * (float)delta;

        // ConstantLinearVelocity = velocity;
        // MoveAndCollide(ConstantLinearVelocity);
    }

    public void CheckWall(Camera2D camera)
    {
        Vector2 pos = GlobalPosition;

        float left = camera.LimitLeft + 16;
        float right = camera.LimitRight - 50;

        // Clamp to camera limits
        float clampedX = Mathf.Clamp(pos.X, left, right);

        GlobalPosition = new Vector2(clampedX, pos.Y);
    }
}
