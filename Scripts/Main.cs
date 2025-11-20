using System.Linq;
using Godot;

namespace MyGame.Scripts;

public partial class Main : Node2D
{
    /**
    
    MAIN GAME LOGIC HERE

    ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡤⠖⠚⠉⠁⠀⠀⠉⠙⠒⢄⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⠔⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢢⡀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡰⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣆⠀⠀
⠀⠀⠀⠀⠀⢠⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢇⠀
⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡄
⠀⠀⠀⠀⢸⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠇
⠀⠀⠀⠀⠸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠐⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡘
⠀⠀⠀⠀⠀⢻⠀⠀⠀⠀⠀⠀⠀⢃⣴⣶⡄⠀⠀⠀⠀⠀⢀⣶⡆⠀⢠⠇
⠀⠀⠀⠀⠀⠀⣣⠀⠀⠀⠀⠀⠀⠀⠙⠛⠁⠀⠀⠀⠀⠀⠈⠛⠁⡰⠃⠀
⠀⠀⠀⠀⢠⠞⠋⢳⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠜⠁⠀⠀
⠀⠀⠀⣰⠋⠀⠀⠁⢷⠙⠲⢤⣀⣀⠀⠀⠀⠀⠴⠴⣆⠴⠚⠁⠀⠀⠀⠀
⠀⠀⣰⠃⠀⠀⠀⠀⠘⡇⠀⣀⣀⡉⠙⠒⠒⠒⡎⠉⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢠⠃⠀⠀⢶⠀⠀⠀⢳⠋⠁⠀⠙⢳⡠⠖⠚⠑⠲⡀⠀⠀⠀⠀⠀⠀⠀
⠀⡎⠀⠀⠀⠘⣆⠀⠀⠈⢧⣀⣠⠔⡺⣧⠀⡴⡖⠦⠟⢣⠀⠀⠀⠀⠀⠀
⢸⠀⠀⠀⠀⠀⢈⡷⣄⡀⠀⠀⠀⠀⠉⢹⣾⠁⠁⠀⣠⠎⠀⠀⠀⠀⠀⠀
⠈⠀⠀⠀⠀⠀⡼⠆⠀⠉⢉⡝⠓⠦⠤⢾⠈⠓⠖⠚⢹⠀⠀⠀⠀⠀⠀⠀
⢰⡀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⢸⠀⠀⠀⠀⡏⠀⠀⠀⠀⠀⠀⠀
⠀⠳⡀⠀⠀⠀⠀⠀⠀⣀⢾⠀⠀⠀⠀⣾⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠈⠐⠢⠤⠤⠔⠚⠁⠘⣆⠀⠀⢠⠋⢧⣀⣀⡼⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠈⠁⠀⠀⠀⠁⠀⠀⢀⡀⠠⠀⠀⠀⠀

In the Brick-End, nothing really bounces the way you planned.

You try your best, but in the Paddle-End, the ball still betrays you.

In the Break-End, every brick acts like it pays rent.

In the End-Brick moment, gravity shows who the real boss is.

You line up the perfect shot, and in the End-Bounce, the ball says ‘nah’.

In the Brick-End, your paddle cries quietly in the corner.

Every level starts with hope, and in the End-Break, it ends with chaos.

In the Bounce-End, the last brick always hides behind its friends.

You swing left, swing right, but in the End-Paddle, the ball still escapes.

In the Final-End-Brick, your aim suddenly forgets how aim works
     
    
    
    */
    private Paddle paddle;

    private Camera2D camera2D;

    private Ball ball;

    private Heart[] hearts;

    private Pause pause;

    private RichTextLabel scoreLabel;

    private GameOver gameOver;

    private AudioStreamPlayer2D gameOverSound,
        victorySound;
    private int totalScore = 0,
        lives = 3;

    private Node2D bricksContainer,
        pauseNode,
        misc,
        gameOverNode;

    private bool wasOutside = false,
        isGameOver = false,
        gameOverTriggered = false;

    public override void _Ready()
    {
        base._Ready();

        ProcessMode = ProcessModeEnum.Always;
        GetWindow().Mode = Window.ModeEnum.Windowed;
        GetWindow().SetFlag(Window.Flags.ResizeDisabled, true);

        misc = GetNode<Node2D>("Misc");

        var heartContainer = misc.GetNode<Node>("Hearts");
        hearts = [.. heartContainer.GetChildren().OfType<Heart>()];

        scoreLabel = misc.GetNode<RichTextLabel>("RichTextLabel");

        camera2D = GetNode<Camera2D>("Camera2D");

        ball = GetNode<Ball>("Ball");

        paddle = GetNode<Paddle>("Paddle");
        paddle.BallEntity = ball;

        pauseNode = GetNode<Node2D>("Node2D");

        gameOverNode = GetNode<Node2D>("GameOver");
        gameOver = gameOverNode.GetNode<GameOver>("GameOver");
        gameOverSound = gameOverNode.GetNode<AudioStreamPlayer2D>("Voice");
        victorySound = gameOverNode.GetNode<AudioStreamPlayer2D>("Victory");

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
        /**
 *
 *      ▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄           ▄▄      ▄▄   ▄▄▄▄    ▄▄▄▄▄▄    ▄▄   ▄▄▄    ▄▄▄▄
 *      ▀▀██▀▀   ▀▀▀██▀▀▀           ██      ██  ██▀▀██   ██▀▀▀▀██  ██  ██▀   ▄█▀▀▀▀█
 *        ██        ██              ▀█▄ ██ ▄█▀ ██    ██  ██    ██  ██▄██     ██▄
 *        ██        ██               ██ ██ ██  ██    ██  ███████   █████      ▀████▄
 *        ██        ██               ███▀▀███  ██    ██  ██  ▀██▄  ██  ██▄        ▀██
 *      ▄▄██▄▄      ██               ███  ███   ██▄▄██   ██    ██  ██   ██▄  █▄▄▄▄▄█▀
 *      ▀▀▀▀▀▀      ▀▀               ▀▀▀  ▀▀▀    ▀▀▀▀    ▀▀    ▀▀▀ ▀▀    ▀▀   ▀▀▀▀▀
 *
 *
 */

        base._Process(delta);

        // GD.Print($"Bricks left: {bricksContainer.GetChildCount()}");
        if (bricksContainer.GetChildCount() <= 1 && !gameOverTriggered)
        {
            GD.Print("Level Complete!");
            gameOverTriggered = true;
            isGameOver = true;
            gameOver.Win = true;

            victorySound.ProcessMode = ProcessModeEnum.Always;
            victorySound.Play();
            victorySound.Finished += victorySound.QueueFree;

            CallDeferred(nameof(FinishGameOver));
        }

        if (ball.IsOutside && !wasOutside && lives > 0)
        {
            lives--;
            hearts[lives].Hurt(); // runs exactly once ever
        }

        wasOutside = ball.IsOutside;

        isGameOver = lives <= 0;
        if (isGameOver && !gameOverTriggered)
        {
            gameOver.Win = false;
            gameOverTriggered = true; // prevents spam

            gameOverSound.ProcessMode = ProcessModeEnum.Always;
            gameOverSound.Play();
            gameOverSound.Finished += gameOverSound.QueueFree;

            CallDeferred(nameof(FinishGameOver));
        }

        if (GetTree().Paused || isGameOver)
        {
            paddle.MoveIt = false;
            pauseNode.Visible = !isGameOver && !gameOver.Win;
            GD.Print($"Game Paused: pauseNode.Visible = {pauseNode.Visible}");
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

    private void FinishGameOver()
    {
        gameOverNode.Visible = true;
        gameOver.FinalScore = totalScore;
        GetTree().Paused = true;
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
