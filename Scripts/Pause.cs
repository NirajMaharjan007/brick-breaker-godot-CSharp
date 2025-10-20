using Godot;

namespace MyGame.Scripts;

public partial class Pause : Control
{
    [Signal]
    public delegate void ResumePressedEventHandler();

    [Signal]
    public delegate void ExitPressedEventHandler();
    private VBoxContainer mainContainer,
        boxContainer;

    private TextureButton play,
        exit;

    private Label label;

    public override void _Ready()
    {
        base._Ready();

        mainContainer = GetNode<VBoxContainer>("Main");
        label = mainContainer.GetNode<Label>("Label");
        boxContainer = mainContainer.GetNode<VBoxContainer>("ButtonContainer");
        play = boxContainer.GetNode<TextureButton>("PlayButton");
        play.Pressed += () =>
        {
            EmitSignal(SignalName.ResumePressed);
        };

        exit = boxContainer.GetNode<TextureButton>("ExitButton");
        exit.Pressed += () =>
        {
            EmitSignal(SignalName.ExitPressed);
        };
    }
}
