using System;
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
    public float Width =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;

    public float Height =>
        (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;

    public override void _Ready()
    {
        base._Ready();

        GD.Print($"Brick Width {Width} and height {Height}");

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

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
