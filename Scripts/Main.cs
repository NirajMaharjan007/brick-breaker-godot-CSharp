using Godot;

namespace MyGame.Scripts;

public partial class Main : Node2D
{
    private Paddle paddle;

    private Camera2D camera2D;

    private Ball ball;

    private Node2D bricksContainer,
        pauseNode;

    public override void _Ready()
    {
        base._Ready();

        ProcessMode = ProcessModeEnum.Always;

        camera2D = GetNode<Camera2D>("Camera2D");

        paddle = GetNode<Paddle>("Paddle");

        ball = GetNode<Ball>("Ball");

        pauseNode = GetNode<Node2D>("Node2D");

        bricksContainer = GetNode<Node2D>("BricksContainer");

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
                brick.Position = new Vector2(x * brick.Width, y * brick.Height);
                bricksContainer.AddChild(brick);
            }
        }

        // GetTree().Paused = true;
    }

    public override void _Process(double delta)
    {
        // TODO: LEVEL CHANGE LOGIC

        base._Process(delta);

        GD.Print($"Bricks left: {bricksContainer.GetChildCount()}");
        if (bricksContainer.GetChildCount() == 0)
        {
            GD.Print("Level Complete!");
        }

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
