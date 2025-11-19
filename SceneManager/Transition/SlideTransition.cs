using System;
using System.Threading.Tasks;
using Godot;

namespace MyGame.ScenesManager;

public partial class SlideTransition : ColorRect
{
    [Signal]
    public delegate void TransitionFinishedEventHandler();

    private float duration = 0.5f;
    private Vector2 startPosition;

    public override void _Ready()
    {
        Size = GetViewport().GetVisibleRect().Size;
        startPosition = Position;
    }

    public async void PlayIn()
    {
        Position = new Vector2(-Size.X, 0);
        var tween = CreateTween();
        tween.TweenProperty(this, "position:x", startPosition.X, duration);
        await ToSignal(tween, "finished");
        EmitSignal(SignalName.TransitionFinished);
    }

    public async void PlayOut()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "position:x", -Size.X, duration);
        await ToSignal(tween, "finished");
        EmitSignal(SignalName.TransitionFinished);
    }

    public void SetDuration(float newDuration) => duration = newDuration;
}
