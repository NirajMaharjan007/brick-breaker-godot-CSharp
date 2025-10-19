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
            breakSound.Autoplay = true;
            breakSound.Play();
            GD.Print($"Playing break sound {breakSound.Playing}");
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Ball && breakSound.Playing)
        {
            breakSound.Autoplay = false;
            GD.Print($"Stopping break sound {breakSound.Playing}");
        }
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
