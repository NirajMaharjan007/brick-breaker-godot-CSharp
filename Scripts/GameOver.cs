using Godot;

namespace MyGame.Scripts;

public partial class GameOver : Control
{
    private AudioStreamPlayer2D gameOverSound;
    private RichTextLabel finalScoreLabel;
    private TextureButton menu,
        exit;

    public int FinalScore { get; set; } = 0;

    public override void _Ready()
    {
        base._Ready();

        var vbox = GetNode<VBoxContainer>("VBoxContainer");
        var buttonsBox = vbox.GetNode<VBoxContainer>("Buttons");

        finalScoreLabel = vbox.GetNode<RichTextLabel>("HighScore");
        finalScoreLabel.Text = $"High Score: {FinalScore}";

        gameOverSound = GetNode<AudioStreamPlayer2D>("Voice");

        menu = buttonsBox.GetNode<TextureButton>("MainMenu");
        menu.Pressed += OnMenuPressed;

        exit = buttonsBox.GetNode<TextureButton>("Exit");
        exit.Pressed += OnExitPressed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        finalScoreLabel.Text = $"Final Score: {FinalScore}";
    }

    private void OnMenuPressed()
    {
        string scenePath = "res://Scenes/Menu/Menu.tscn";

        var mainScene = GD.Load<PackedScene>(scenePath);
        if (mainScene == null)
        {
            GD.PrintErr($"Failed to load scene at path: {scenePath}");
            return;
        }
        GetTree().ChangeSceneToPacked(mainScene);
        QueueFree();
    }

    private void OnExitPressed()
    {
        GD.Print("Exiting game...\nBye!....");
        try
        {
            GetTree().Quit();
            System.Environment.Exit(0);
        }
        catch (System.Exception e)
        {
            GD.PushError(e);
            GD.PrintErr(e.ToString());
            System.Environment.Exit(1);
        }
    }
}
