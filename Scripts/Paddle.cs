using Godot;

namespace MyGame.Scripts;

public partial class Paddle : StaticBody2D
{
    private const int SPEED = 200;

    public override void _Ready()
    {
        base._Ready();
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
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_right"))
            velocity.X += 1;

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
