using Godot;

namespace MyGame.Scripts;

public partial class Ball : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    // public override void _PhysicsProcess(double delta)
    // {
    //     base._PhysicsProcess(delta);
    // }

    public void CheckWall(Camera2D camera)
    {
        Vector2 pos = GlobalPosition;

        float left = camera.LimitLeft + 16;
        float right = camera.LimitRight - 50;

        float bottom = camera.LimitBottom + 50;
        float top = camera.LimitTop;

        // Clamp to camera limits
        float clampedX = Mathf.Clamp(pos.X, left, right);
        float clampedY = Mathf.Clamp(pos.Y, top, bottom);

        // GlobalPosition = new Vector2(clampedX, clampedY);
    }

    public void PaddleHit(Paddle paddle) { }
}
