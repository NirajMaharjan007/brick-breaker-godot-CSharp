using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    private const int SPEED = 300;

    private Area2D area2D;

    public override void _Ready()
    {
        base._Ready();
        area2D = GetNode<Area2D>("Area2D");
        area2D.BodyEntered += OnBodyEntered;

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

    private void PaddleHit(Paddle paddle)
    {
        // Bounce direction (X depends on hit position, Y always up)
        Vector2 newVelocity = new Vector2(0, -1).Normalized() * SPEED;

        LinearVelocity = newVelocity;
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print($"THIS BODY -> {body.Name}");
        if (body.Name.ToString().Equals("Paddle"))
        {
            PaddleHit(body as Paddle);
        }
    }

    public void HitSound() { }
}
