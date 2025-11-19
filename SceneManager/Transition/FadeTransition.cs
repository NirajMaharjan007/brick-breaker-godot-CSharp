using System;
using System.Threading.Tasks;
using Godot;

namespace MyGame.ScenesManager.Transition;

public partial class FadeTransition : ColorRect
{
    [Signal]
    public delegate void TransitionFinishedEventHandler();

    private float duration = 0.5f;
    private Color transitionColor = new(0, 0, 0, 0);

    public override void _Ready()
    {
        Color = transitionColor;
        Size = GetViewport().GetVisibleRect().Size;
    }

    public async void PlayIn()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "color:a", 0.0f, duration);
        await ToSignal(tween, "finished");
        EmitSignal(SignalName.TransitionFinished);
    }

    public async void PlayOut()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "color:a", 1.0f, duration);
        await ToSignal(tween, "finished");
        EmitSignal(SignalName.TransitionFinished);
    }

    public void SetDuration(float newDuration) => duration = newDuration;

    public void SetTransitionColor(Color color)
    {
        transitionColor = color;
        Color = new Color(color.R, color.G, color.B, Color.A);
    }
}
