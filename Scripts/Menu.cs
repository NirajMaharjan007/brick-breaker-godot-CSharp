using Godot;

namespace MyGame.Scripts;

public partial class Menu : Control
{
    private VBoxContainer container;
    private Button startButton;
    private Button exitButton;

    public override void _Ready()
    {
        container = GetNode<VBoxContainer>("VBoxContainer");

        var buttonContainer = container.GetNode<VBoxContainer>("ButtonContainer");

        startButton = buttonContainer.GetNode<Button>("StartButton");
        startButton.Pressed += OnStartButtonPressed;

        exitButton = buttonContainer.GetNode<Button>("ExitButton");
        exitButton.Pressed += OnExitButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        var mainScene = GD.Load<PackedScene>("res://Scenes/Menu/Loading.tscn");
        if (mainScene == null)
        {
            GD.PrintErr("Failed to load 'Loading' scene.");
            return;
        }

        var mainNode = mainScene.Instantiate();
        if (mainNode == null)
        {
            GD.PrintErr("Failed to instantiate 'Loading' scene.");
            return;
        }

        GetTree().Root.AddChild(mainNode);
        QueueFree();

        // var label = container.GetNode<Label>("Label");
        // label.Text = "Start Button Pressed!";
        // GD.Print("Start Button Pressed!");
    }

    private void OnExitButtonPressed()
    {
        try
        {
            GetTree().Quit();
            System.Environment.Exit(0);
        }
        catch (System.Exception e)
        {
            GD.PushError(e);
            GD.PrintErr($"Error Exiting Game {e.Message}");
            System.Environment.Exit(-1);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (this is not null)
        {
            try
            {
                if (startButton is not null)
                {
                    startButton.Pressed -= OnStartButtonPressed;
                    startButton.Dispose();
                }

                if (exitButton is not null)
                {
                    exitButton.Pressed -= OnExitButtonPressed;
                    exitButton.Dispose();
                }

                container?.Dispose();
            }
            catch (System.Exception e)
            {
                GD.PrintErr($"Error -> {e.ToString()}");
                System.Environment.Exit(-1);
            }
            finally
            {
                Visible = false;
                Dispose();
            }
        }
    }
}
