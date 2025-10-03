using Godot;

namespace MyGame.Scripts;

public partial class Loading : Node
{
    private VBoxContainer container;
    private ProgressBar progressBar;

    private double targetValue = 0;

    public override void _Ready()
    {
        base._Ready();
        GD.Print("Loading...");

        container = GetNode<VBoxContainer>("VBoxContainer");
        progressBar = container.GetNode<ProgressBar>("ProgressBar");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        targetValue += 100;
        progressBar.Value = Mathf.MoveToward(
            (float)progressBar.Value,
            (float)targetValue,
            (float)(32 * delta) // speed of fill
        );

        GD.Print($"Progress: {progressBar.Value}/{progressBar.MaxValue}");
        GD.Print($"Target: {targetValue}");

        if (progressBar.Value >= progressBar.MaxValue)
        {
            GetTree().ChangeSceneToFile("res://Scenes/Game/Main.tscn");
            QueueFree();
        }
    }
}
