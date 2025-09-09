using Godot;

namespace MyGame.Scripts;

public partial class Main : Node2D
{
    private Paddle paddle;

    private Camera2D camera2D;

    private Ball ball;

    public override void _Ready()
    {
        base._Ready();

        paddle = GetNode<Paddle>("Paddle");
        ball = GetNode<Ball>("Ball");
        camera2D = GetNode<Camera2D>("Camera2D");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        paddle.CheckWall(camera2D);
        ball.CheckWall(camera2D);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
