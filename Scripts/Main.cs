using System;
using System.Linq;
using Godot;

namespace MyGame.Scripts;

public partial class Main : Node2D
{
    private Paddle paddle;

    private Camera2D camera2D;

    private Ball ball;

    private Heart heart;

    private Pause pause;

    private RichTextLabel scoreLabel;

    private int totalScore = 0;

    private Node2D bricksContainer,
        pauseNode,
        misc;

    private bool wasOutside = false,
        hasTriggered = false;

    public override void _Ready()
    {
        base._Ready();

        ProcessMode = ProcessModeEnum.Always;

        misc = GetNode<Node2D>("Misc");

        var heartContainer = misc.GetNode<Node2D>("Hearts");
        heart = heartContainer.GetNode<Heart>("Heart3");

        scoreLabel = misc.GetNode<RichTextLabel>("RichTextLabel");

        camera2D = GetNode<Camera2D>("Camera2D");

        ball = GetNode<Ball>("Ball");

        paddle = GetNode<Paddle>("Paddle");
        paddle.BallEntity = ball;

        pauseNode = GetNode<Node2D>("Node2D");

        bricksContainer = GetNode<Node2D>("BricksContainer");

        pause = pauseNode.GetNode<Pause>("Pause");
        pause.ResumePressed += () =>
        {
            GetTree().Paused = false;
        };
        pause.ExitPressed += () =>
        {
            GetTree().Quit();
            System.Environment.Exit(0);
        };

        InitializedBricks();
        // InitializedHeart();
    }

    private void InitializedHeart()
    {
        var heartContainer = misc.GetNode<Area2D>("Hearts");
        var heartScene = GD.Load<PackedScene>("res://Scenes/Game/Heart.tscn");

        var areaSize =
            heartContainer.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        float heartWidth = heartScene.Instantiate<Heart>().Width;
        // int heartsCount = (int)(areaSize.Size.X / heartWidth);
        GD.Print($"Hearts area size: {areaSize.Size}, Heart Width: {heartWidth}");
    }

    private void InitializedBricks()
    {
        var shape =
            bricksContainer
                .GetNode<Area2D>("Area2D")
                .GetNode<CollisionShape2D>("CollisionShape2D")
                .Shape as RectangleShape2D;

        var areaSize = shape.Size;

        var brickScene = GD.Load<PackedScene>("res://Scenes/Game/Brick.tscn");

        var tempBrick = brickScene.Instantiate<Brick>();
        float brickWidth = tempBrick.Width;
        float brickHeight = tempBrick.Height;
        tempBrick.QueueFree();

        int col = (int)(areaSize.X / brickWidth);
        int row = (int)(areaSize.Y / brickHeight);

        GD.Print($"Brick area size: {areaSize}");
        GD.Print($"Rows: {row}, Cols: {col}");

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                var brick = brickScene.Instantiate<Brick>();
                brick.BrickDestroyed += OnBrickDestroyed;
                brick.Position = new Vector2(x * brick.Width, y * brick.Height);
                bricksContainer.AddChild(brick);
            }
        }
    }

    private void OnBrickDestroyed(int score)
    {
        totalScore += score;
        scoreLabel.Text = $"Score: {totalScore}";
        GD.Print($"Score updated → {totalScore}");
    }

    public override void _Process(double delta)
    {
        // TODO: LEVEL CHANGE LOGIC

        base._Process(delta);

        GD.Print($"Flag: {ball.IsOutside}, WasOutside: {wasOutside}, HasTriggered: {hasTriggered}");
        if (bricksContainer.GetChildCount() <= 1)
        {
            GD.Print("Level Complete!");
        }

        if (ball.IsOutside && !wasOutside && !hasTriggered)
        {
            heart.Hurt(); // runs exactly once ever
            hasTriggered = true;
        }

        wasOutside = ball.IsOutside;

        if (GetTree().Paused)
        {
            paddle.MoveIt = false;
            pauseNode.Visible = true;
        }
        else
        {
            paddle.MoveIt = true;
            pauseNode.Visible = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        paddle.CheckWall(camera2D);
        ball.CheckWall(camera2D);
    }

    private void TogglePause()
    {
        if (GetTree().Paused)
        {
            // Unpause
            GetTree().Paused = false;
            pause.Visible = false;
            paddle.MoveIt = true;
        }
        else
        {
            // Pause
            GetTree().Paused = true;
            pause.Visible = true;
            paddle.MoveIt = false;
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("ui_cancel")) // e.g. ESC key
        {
            GetTree().Paused = !GetTree().Paused;
            GD.Print($"Game paused: {GetTree().Paused}");
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        try
        {
            if (this is not null)
            {
                Dispose();
                System.Environment.Exit(0);
            }
        }
        catch (System.Exception e)
        {
            GD.PushError(e);
            GD.PrintErr(e.ToString());
            System.Environment.Exit(-1);
        }
    }
}
