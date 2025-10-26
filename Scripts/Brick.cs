using Godot;

namespace MyGame.Scripts;

public partial class Brick : StaticBody2D
{
    private static class ResourcesPath
    {
        public const string BLACK = "res://Assests/break assets/bricks/sp_brick_black.png",
            WHITE = "res://Assests/break assets/bricks/sp_brick_white.png",
            RED = "res://Assests/break assets/bricks/sp_brick_red.png",
            GREEN = "res://Assests/break assets/bricks/sp_brick_green.png",
            ORANGE = "res://Assests/break assets/bricks/sp_brick_orange.png",
            PINK = "res://Assests/break assets/bricks/sp_brick_pink.png",
            PURPLE = "res://Assests/break assets/bricks/sp_brick_purple.png";
    }

    public interface IScore
    {
        public const int WHITE = 70,
            BLACK = 60,
            RED = 50,
            GREEN = 40,
            ORANGE = 30,
            PINK = 20,
            PURPLE = 10;
    };

    public int Score { get; private set; } = 0;

    private Sprite2D sprite;

    private AudioStreamPlayer2D breakSound;

    private Area2D area2D;
    public float Width =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;

    public float Height =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;

    private CollisionShape2D collisionShape2D;

    public override void _Ready()
    {
        base._Ready();

        // GD.Print($"Brick Width {Width} and height {Height}");

        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

        breakSound = GetNode<AudioStreamPlayer2D>("Break");
        breakSound.Autoplay = false;
        breakSound.Bus = "SFX";

        area2D = GetNode<Area2D>("Area2D");
        area2D.BodyEntered += OnBodyEntered;
        area2D.BodyExited += OnBodyExited;

        Init();
    }

    private void Init()
    {
        int value = GD.RandRange(1, 7);

        sprite = GetNode<Sprite2D>("Sprite2D");

        switch (value)
        {
            case 1:
                var texture = GD.Load<Texture2D>(ResourcesPath.BLACK);
                sprite.Texture = texture;
                break;

            case 2:
                texture = GD.Load<Texture2D>(ResourcesPath.WHITE);
                sprite.Texture = texture;
                break;

            case 3:
                texture = GD.Load<Texture2D>(ResourcesPath.ORANGE);
                sprite.Texture = texture;
                break;

            case 4:
                texture = GD.Load<Texture2D>(ResourcesPath.PINK);
                sprite.Texture = texture;
                break;

            case 5:
                texture = GD.Load<Texture2D>(ResourcesPath.RED);
                sprite.Texture = texture;
                break;

            case 6:
                texture = GD.Load<Texture2D>(ResourcesPath.GREEN);
                sprite.Texture = texture;
                break;

            case 7:
                texture = GD.Load<Texture2D>(ResourcesPath.PURPLE);
                sprite.Texture = texture;
                break;
        }
    }

    public void DisableBrickCompletely()
    {
        // For Area2D bricks
        Visible = false;

        // Disable collision shape
        // collisionShape2D.CallDeferred("set_disabled", true);
        // collisionShape2D.SetDeferred("disabled", true);
        Callable.From(() => collisionShape2D.Disabled = true).CallDeferred();
        // Stop all processing
        ProcessMode = ProcessModeEnum.Disabled;

        CallDeferred("queue_free");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    private void OnBodyEntered(Node2D body)
    {
        // GD.Print($"Brick Body Entered by {body is Ball}");

        if (body is Ball)
        {
            GD.Print("Brick Hit by Ball");
            PlayBreakSound();
            GD.Print($"Playing break sound {breakSound.Playing}");
        }
    }

    private void OnBodyExited(Node2D body)
    {
        // TODO: FIX ME
        // if (body is Ball && breakSound.Playing)
        // {
        //     GD.Print($"Stopping break sound {breakSound.Playing}");
        // }

        if (body is Ball)
        {
            var texture = sprite.Texture;
            GD.Print($"Texture: {texture.ResourcePath}");
            switch (texture.ResourcePath)
            {
                case ResourcesPath.BLACK:
                    GD.Print(IScore.BLACK);
                    Score = IScore.BLACK;
                    break;

                case ResourcesPath.WHITE:
                    GD.Print(IScore.WHITE);
                    Score = IScore.WHITE;
                    break;

                case ResourcesPath.ORANGE:
                    GD.Print(IScore.ORANGE);
                    Score = IScore.ORANGE;
                    break;

                case ResourcesPath.PINK:
                    GD.Print(IScore.PINK);
                    Score = IScore.PINK;
                    break;

                case ResourcesPath.RED:
                    GD.Print(IScore.RED);
                    Score = IScore.RED;
                    break;

                case ResourcesPath.GREEN:
                    GD.Print(IScore.GREEN);
                    Score = IScore.GREEN;
                    break;

                case ResourcesPath.PURPLE:
                    GD.Print(IScore.PURPLE);
                    Score = IScore.PURPLE;
                    break;
            }
        }
    }

    private void PlayBreakSound()
    {
        if (breakSound.Playing)
            breakSound.Stop();

        breakSound.PitchScale = (float)GD.RandRange(0.95f, 1.05f); // Subtle variation
        breakSound.VolumeDb = (float)GD.RandRange(-5.0f, 0.0f); // Slight volume variation
        breakSound.Play();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        try
        {
            if (this is not null)
            {
                if (area2D is not null)
                {
                    area2D.BodyEntered -= OnBodyEntered;
                    area2D.BodyExited -= OnBodyExited;
                    area2D.Dispose();
                }
            }
        }
        catch (System.Exception e)
        {
            GD.PushError(e);
            GD.PrintErr($"Error -> {e.ToString()}");
        }
        finally
        {
            Visible = false;
            QueueFree();
            Dispose();
        }
    }
}
