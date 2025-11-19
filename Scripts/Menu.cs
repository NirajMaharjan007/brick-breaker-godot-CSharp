using Godot;
using MyGame.ScenesManager;

namespace MyGame.Scripts;

public partial class Menu : Control
{
    private VBoxContainer container;
    private TextureButton startButton;
    private TextureButton exitButton;

    private GodotObject sceneManager;

    public override void _Ready()
    {
        base._Ready();

        GetWindow().Mode = Window.ModeEnum.Windowed;
        GetWindow().SetFlag(Window.Flags.ResizeDisabled, true);

        container = GetNode<VBoxContainer>("VBoxContainer");

        var buttonContainer = container.GetNode<VBoxContainer>("ButtonContainer");

        startButton = buttonContainer.GetNode<TextureButton>("StartButton");
        startButton.Pressed += OnStartButtonPressed;

        exitButton = buttonContainer.GetNode<TextureButton>("ExitButton");
        exitButton.Pressed += OnExitButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        var sceneManager = SceneManager.Instance;
        // SceneManager.Instance.ChangeSceneWithTransition(
        //     "res://Scenes/Game/Main.tscn",
        //     "fade",
        //     1.0f
        // );

        if (sceneManager is null)
            GD.PrintErr("SceneManager Instance is NULL");
        else
        {
            GD.Print("SceneManager Instance FOUND!!!!");
            sceneManager.ChangeSceneWithTransition("res://Scenes/Menu/Loading.tscn", "fade", 1.24f);
        }

        /* var mainScene = GD.Load<PackedScene>("res://Scenes/Menu/Loading.tscn");
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
        QueueFree(); */
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
            System.Environment.Exit(1);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (startButton is not null)
            startButton.Pressed -= OnStartButtonPressed;

        if (exitButton is not null)
            exitButton.Pressed -= OnExitButtonPressed;
    }
}
