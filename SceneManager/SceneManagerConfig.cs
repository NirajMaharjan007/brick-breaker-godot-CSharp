using Godot;

namespace MyGame.ScenesManager;

[GlobalClass]
public partial class SceneManagerConfig : Resource
{
    [Export]
    public float DefaultTransitionDuration { get; set; } = 0.5f;

    [Export]
    public Color DefaultTransitionColor { get; set; } = Colors.Black;

    [Export]
    public string DefaultTransitionType { get; set; } = "fade";

    [Export]
    public bool EnableSceneCaching { get; set; } = true;

    [Export]
    public int MaxHistorySize { get; set; } = 10;
}
