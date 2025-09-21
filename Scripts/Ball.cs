using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    private const int SPEED = 300;

    private AudioStreamPlayer2D hitSound;

    private Area2D area2D;

    public override void _Ready()
    {
        base._Ready();
        area2D = GetNode<Area2D>("Area2D");
        area2D.BodyEntered += OnBodyEntered;
        area2D.BodyExited += OnBodyExited;

        hitSound = GetNode<AudioStreamPlayer2D>("HitSound");

        LinearVelocity = new Vector2(0, SPEED);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        GD.Print($"BALLS LINER VELO-> {LinearVelocity}");
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

            int randomY = GD.RandRange(-1, 1); // -1, 0, or 1
            vel.Y += randomY * 64;

            vel.Y += randomY * 64;
        }
        else if (pos.Y >= bottom || pos.Y <= top)
        {
            // Flip Y so it bounces vertically
            vel.Y = -vel.Y;

            // Add random X variation so bounce isn’t predictable
            int randomX = GD.RandRange(-1, 1); // -1, 0, or 1
            vel.X += randomX * 64;
        }

        LinearVelocity = vel.Normalized() * SPEED;

        // GD.Print(pos);
        // GD.Print("VEL " + vel);
    }

    private void Hit()
    {
        Vector2 newVelocity = new Vector2(0, -1).Normalized() * SPEED;

        LinearVelocity = newVelocity;
    }

    private void RandomizeDirection(Brick brick)
    {
        // Get brick center position
        Vector2 brickCenter = brick.GlobalPosition;

        // Calculate hit position relative to brick center
        float relativeHitX = (GlobalPosition.X - brickCenter.X) / (brick.Width / 2);
        float relativeHitY = (GlobalPosition.Y - brickCenter.Y) / (brick.Height / 2);

        // Add randomness based on hit location
        float randomAngle = GD.RandRange(-1, 1); // Small random variation

        // Calculate new direction based on hit position
        Vector2 newDirection = LinearVelocity.Normalized();

        // Add horizontal influence based on where ball hit the brick
        newDirection.X += relativeHitX * 0.3f;
        newDirection.Y += relativeHitY * 0.2f;

        // Add random variation
        newDirection.X += randomAngle;
        newDirection.Y += randomAngle * 0.5f;

        // Normalize and apply
        LinearVelocity = newDirection.Normalized() * LinearVelocity.Length();

        // Optional: Small speed increase
        LinearVelocity *= 1.02f;
    }

    private void OnBodyEntered(Node body)
    {
        // GD.Print(body.Name + $" Brick {body is Brick}");

        if (body.Name.ToString().Equals("Paddle"))
        {
            Hit();
        }
        else if (body is Brick brick)
        {
            RandomizeDirection(brick);
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Brick brick)
        {
            brick.DisableBrickCompletely();
            brick.QueueFree();
            brick.Dispose();
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (area2D is not null)
        {
            area2D.BodyEntered -= OnBodyEntered;
            area2D.BodyExited -= OnBodyExited;
        }
    }
}
