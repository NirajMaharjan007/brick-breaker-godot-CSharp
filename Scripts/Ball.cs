using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    private const int SPEED = 300;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += (Node body) =>
        {
            GD.Print($"body {body.Name}");
        };
        LinearVelocity = new Vector2(0, -SPEED);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!LinearVelocity.IsZeroApprox())
            LinearVelocity = LinearVelocity.Normalized() * SPEED;
    }

    public void CheckWall(Camera2D camera)
    {
        Vector2 pos = Position;

        float left = camera.LimitLeft + 16;
        float right = camera.LimitRight - 50;

        float bottom = camera.LimitBottom - 150;
        float top = camera.LimitTop;

        var vel = LinearVelocity;

        if (pos.X <= left || pos.X >= right)
        {
            vel.X = -vel.X;
        }
        else if (pos.Y > bottom || pos.Y < top)
        {
            vel.Y = -vel.Y;
        }

        LinearVelocity = vel.Normalized() * SPEED;

        // GD.Print(pos);
        // GD.Print("VEL " + vel);
    }

    public void PaddleHit(Paddle paddle)
    {
        float hitPosition = (GlobalPosition.X - paddle.GlobalPosition.X) / (paddle.Width / 2f);
        hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);

        // Bounce direction (X depends on hit position, Y always up)
        Vector2 newVelocity = new Vector2(hitPosition, -1).Normalized() * SPEED;

        LinearVelocity = newVelocity;
    }
}
