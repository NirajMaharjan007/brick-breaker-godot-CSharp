using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    private const int SPEED = 300;

    private AudioStreamPlayer2D hitSound;

    private Area2D area2D;

    private Sprite2D sprite;

    private Paddle Entity { set; get; }

    public static int Speed => SPEED;

    private static class BallColor
    {
        public const string RED = "res://Assests/break assets/misc/red_ball.png",
            YELLOW = "res://Assests/break assets/misc/yellow_ball.png",
            BLUE = "res://Assests/break assets/misc/blue_ball.png",
            ORANGE = "res://Assests/break assets/misc/orange_ball.png";
    }

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite2D>("Sprite2D");

        int value = GD.RandRange(1, 4);
        string path = value switch
        {
            1 => BallColor.RED,
            2 => BallColor.BLUE,
            3 => BallColor.ORANGE,
            4 => BallColor.YELLOW,
            _ => BallColor.RED,
        };

        sprite.Texture = GD.Load<Texture2D>(path);
        GD.Print($"BALL TEXTURE {sprite.Texture.ResourcePath} Value {value}");

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
        else if (LinearVelocity.IsZeroApprox())
        {
            LinearVelocity = new Vector2(0, SPEED);
        }
    }

    public void CheckWall(Camera2D camera)
    {
        Vector2 pos = Position;

        float left = camera.LimitLeft;
        float right = camera.LimitRight - 32;

        float bottom = camera.LimitBottom - 150;
        float top = camera.LimitTop + 4;

        var vel = LinearVelocity;

        // --- LEFT / RIGHT bounce ---
        if (pos.X <= left)
        {
            pos.X = left + 4; // push slightly away from wall
            vel.X = System.MathF.Abs(vel.X); // ensure moving right
            vel.Y += GD.RandRange(-64, 64);
        }
        else if (pos.X >= right)
        {
            pos.X = right - 4;
            vel.X = -System.MathF.Abs(vel.X); // ensure moving left
            vel.Y += GD.RandRange(-64, 64);
        }
        // --- TOP bounce ---
        else if (pos.Y <= top)
        {
            pos.Y = top + 4;
            vel.Y = System.MathF.Abs(vel.Y); // ensure moving down
            vel.X += GD.RandRange(-64, 64);
        }
        else if (pos.Y >= bottom)
        {
            try
            {
                if (Entity is not null)
                    pos = new Vector2(Entity.Position.X, Entity.Position.Y - 20);
                else
                    throw new("Error Paddle Entity");
            }
            catch (System.Exception e)
            {
                pos = new Vector2(170.0f, 250.0f);
                GD.PushError(e);
                GD.PrintErr($"Error -> {e.ToString()}");
            }
        }

        Position = pos;
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
    }

    private void OnBodyEntered(Node body)
    {
        // GD.Print(body.Name + $" Brick {body is Brick}");

        if (body is Paddle paddle)
        {
            Entity = paddle;
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

        if (this is not null)
        {
            try
            {
                if (area2D is not null)
                {
                    area2D.BodyEntered -= OnBodyEntered;
                    area2D.BodyExited -= OnBodyExited;
                    area2D.Dispose();
                }
            }
            catch (System.Exception e)
            {
                GD.PrintErr($"Error -> {e.ToString()}");
            }
            finally
            {
                Visible = false;
                Dispose();
            }
        }
    }
}
