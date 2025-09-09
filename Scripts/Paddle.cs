using Godot;

namespace MyGame.Scripts;

public partial class Paddle : CharacterBody2D
{
    private const int SPEED = 200;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Move();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    private void Move()
    {
        var velocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_left"))
        {
            velocity.X = -SPEED;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.X = SPEED;
        }
        else
            velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED);

        Velocity = velocity;
        MoveAndSlide();
    }

    public void CheckWall(Camera2D camera)
    {
        Vector2 pos = GlobalPosition;

        float left = camera.LimitLeft + 16;
        float right = camera.LimitRight - 50;

        // Clamp to camera limits
        float clampedX = Mathf.Clamp(pos.X, left, right);
        float clampedY = Mathf.Clamp(pos.Y, camera.LimitTop, camera.LimitBottom);

        var str =
            $"camera limitd left and right {left} and {right} respectively\n Paddle->{GlobalPosition}";
        GD.Print(str);

        GlobalPosition = new Vector2(clampedX, clampedY);
    }
}
