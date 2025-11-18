using Godot;

namespace MyGame.Scripts;

public partial class Loading : Node
{
    [Signal]
    public delegate void LoadingCompleteEventHandler();
    private VBoxContainer container;
    private ProgressBar progressBar;

    private double targetValue = 0;

    private bool loadingFinished = false;

    public override void _Ready()
    {
        base._Ready();
        GetWindow().Mode = Window.ModeEnum.Windowed;
        GetWindow().SetFlag(Window.Flags.ResizeDisabled, true);
        // GD.Print("Loading...");

        container = GetNode<VBoxContainer>("VBoxContainer");
        progressBar = container.GetNode<ProgressBar>("ProgressBar");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        targetValue += 5;
        progressBar.Value = Mathf.MoveToward(
            (float)progressBar.Value,
            (float)targetValue,
            (float)(100 * delta)
        // speed of fill
        );

        // GD.Print($"Progress: {progressBar.Value}/{progressBar.MaxValue}");
        //  GD.Print($"Target: {targetValue}");

        if (!loadingFinished && !IsQueuedForDeletion() && progressBar.Value >= progressBar.MaxValue)
        {
            loadingFinished = true;
            OnLoadingComplete();
        }
    }

    private async void OnLoadingComplete()
    {
        // Step 1: Notify completion
        EmitSignal(SignalName.LoadingComplete);

        // Step 2: Wait a frame for receivers to process
        await ToSignal(GetTree(), "process_frame");

        // Step 3: Clean up
        CallDeferred(nameof(RemoveLoader));
    }

    private void RemoveLoader()
    {
        QueueFree();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (progressBar is not null)
        {
            progressBar.Value = 0;
        }
    }
}
