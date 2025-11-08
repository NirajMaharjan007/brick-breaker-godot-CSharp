using System.Collections.Generic;
using Godot;

namespace MyGame.Scripts;

public partial class Brick : StaticBody2D
{
    [Signal]
    public delegate void BrickDestroyedEventHandler(int score);

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

    //For optimization purpose load textures only once
    private static readonly Texture2D[] brickTextures =
    [
        GD.Load<Texture2D>(ResourcesPath.BLACK),
        GD.Load<Texture2D>(ResourcesPath.WHITE),
        GD.Load<Texture2D>(ResourcesPath.ORANGE),
        GD.Load<Texture2D>(ResourcesPath.PINK),
        GD.Load<Texture2D>(ResourcesPath.RED),
        GD.Load<Texture2D>(ResourcesPath.GREEN),
        GD.Load<Texture2D>(ResourcesPath.PURPLE),
    ];

    private void Init()
    {
        //Optimize Version
        sprite = GetNode<Sprite2D>("Sprite2D");
        int index = GD.RandRange(0, brickTextures.Length - 1);
        sprite.Texture = brickTextures[index];

        // GD.Print($"Brick texture assigned: {sprite.Texture.ResourcePath}");
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
         //   GD.Print("Brick Hit by Ball");
            PlayBreakSound();
         //   GD.Print($"Playing break sound {breakSound.Playing}");
        }
    }

    private void OnBodyExited(Node2D body)
    {
        // TODO: FIX ME
        // if (body is Ball && breakSound.Playing)
        // {
        //     GD.Print($"Stopping break sound {breakSound.Playing}");
        // }

        if (body is not Ball)
            return;

        var texturePath = sprite.Texture?.ResourcePath ?? string.Empty;

        // Dictionary maps resource paths directly to scores
        var scoreMap = new Dictionary<string, int>
        {
            { ResourcesPath.BLACK, IScore.BLACK },
            { ResourcesPath.WHITE, IScore.WHITE },
            { ResourcesPath.ORANGE, IScore.ORANGE },
            { ResourcesPath.PINK, IScore.PINK },
            { ResourcesPath.RED, IScore.RED },
            { ResourcesPath.GREEN, IScore.GREEN },
            { ResourcesPath.PURPLE, IScore.PURPLE },
        };

        if (scoreMap.TryGetValue(texturePath, out int value))
        {
            Score = value;
            EmitSignal(SignalName.BrickDestroyed, Score);
            //  GD.Print($"Brick texture: {texturePath}, Score: {Score}");
        }
        else
        {
            GD.PrintErr($"⚠ Unknown texture path: {texturePath}");
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
