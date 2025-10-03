using Godot;

namespace MyGame.Scripts;

public partial class Loading : Node
{
    private VBoxContainer container;
    private ProgressBar progressBar;

    public override void _Ready()
    {
        base._Ready();
        GD.Print("Loading...");

        container = GetNode<VBoxContainer>("VBoxContainer");
        progressBar = container.GetNode<ProgressBar>("ProgressBar");

        var timer = GetNode<Timer>("Timer");
        timer.Timeout += On_Timer_timeout;
        timer.Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        progressBar.Value += 10;
    }

    private void On_Timer_timeout()
    {
        try
        {
            if (progressBar.Value >= progressBar.MaxValue)
            {
                GD.Print("Loading complete!");
                GetTree().ChangeSceneToFile("res://Scenes/Game/Main.tscn");
                QueueFree();
            }
        }
        catch (System.Exception e)
        {
            GD.PushError($"Failed to change scene: {e}");
            GD.PrintErr($"Error during loading: {e.Message}\n{e.StackTrace}");
            System.Environment.Exit(-1);
        }
    }
}
