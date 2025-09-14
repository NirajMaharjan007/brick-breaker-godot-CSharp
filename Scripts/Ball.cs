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
            HitSound();
        }
        else if (pos.Y > bottom || pos.Y < top)
        {
            vel.Y = -vel.Y;
            HitSound();
        }

        LinearVelocity = vel.Normalized() * SPEED;

        // GD.Print(pos);
        // GD.Print("VEL " + vel);
    }

    private void PaddleHit()
    {
        Vector2 newVelocity = new Vector2(0, -1).Normalized() * SPEED;

        LinearVelocity = newVelocity;
    }

    private void BrickHit(Brick brick)
    {
        GD.Print("BRICK width " + brick.Width);

        // Relative hit position (-1 = left, 0 = center, +1 = right)
        float hitPos = (GlobalPosition.X - brick.GlobalPosition.X) / (brick.Width / 2f);
        hitPos = Mathf.Clamp(hitPos, -1f, 1f);

        // New bounce direction (X depends on hit, Y always up)
        Vector2 newVelocity = new Vector2(hitPos, -1).Normalized() * SPEED;

        LinearVelocity = newVelocity;
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print(body.Name);
        if (body.Name.ToString().Equals("Paddle"))
        {
            PaddleHit();
            HitSound();
        }
        else if (body.Name.ToString().Equals("Brick"))
        {
            if (body is Brick brick)
                BrickHit(brick);

            HitSound();
        }
    }

    private void OnBodyExited(Node body)
    {
        HitSoundStop();
    }

    public void HitSound()
    {
        hitSound.Play();
    }

    public void HitSoundStop()
    {
        hitSound.Stop();
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
