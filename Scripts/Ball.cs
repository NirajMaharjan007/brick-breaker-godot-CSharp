using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    private const int SPEED = 300;

    public override void _Ready()
    {
        base._Ready();
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
        float top = camera.LimitTop + 20;

        var vel = LinearVelocity;

        if (pos.X <= left || pos.X >= right)
        {
            vel.X = -vel.X;
        }
        else if (pos.Y > bottom)
        {
            vel.Y = -vel.Y;
        }

        LinearVelocity = vel.Normalized() * SPEED;

        GD.Print(pos);
        GD.Print("VEL " + vel);
    }

    public void PaddleHit(Paddle paddle) { }
}
