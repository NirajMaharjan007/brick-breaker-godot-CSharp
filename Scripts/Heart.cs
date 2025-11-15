using System;
using Godot;

namespace MyGame.Scripts;

public partial class Heart : AnimatedSprite2D
{
    private float width;
    private float height;

    public override void _Ready()
    {
        base._Ready();

        var area = GetNode<Area2D>("Area2D");
        var shape = area.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        width = shape?.Size.X ?? 0f;
        height = shape?.Size.Y ?? 0f;

        AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished()
    {
        // After "hurt", go back to "default"
        if (Animation.Equals("hurt"))
            Play("empty");
    }

    public void Hurt()
    {
        Play("hurt");
    }

    public void FullHeart()
    {
        Play("default");
    }

    public void EmptyHeart()
    {
        Play("empty");
    }

    public float Width => width;
    public float Height => height;
}
