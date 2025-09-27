using Godot;

namespace MyGame.Scripts;

public partial class Main : Node2D
{
    private Paddle paddle;

    private Camera2D camera2D;

    private Ball ball;

    private Node2D bricksContainer;

    public override void _Ready()
    {
        base._Ready();

        camera2D = GetNode<Camera2D>("Camera2D");

        paddle = GetNode<Paddle>("Paddle");

        ball = GetNode<Ball>("Ball");

        bricksContainer = GetNode<Node2D>("BricksContainer");

        var shape =
            bricksContainer
                .GetNode<Area2D>("Area2D")
                .GetNode<CollisionShape2D>("CollisionShape2D")
                .Shape as RectangleShape2D;

        var areaSize = shape.Size;

        var brickScene = GD.Load<PackedScene>("res://Scenes/Brick.tscn");

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
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        paddle.CheckWall(camera2D);
        ball.CheckWall(camera2D);
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
