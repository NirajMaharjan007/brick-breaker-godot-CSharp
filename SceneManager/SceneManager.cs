using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MyGame.ScenesManager.Transition;

namespace MyGame.ScenesManager;

public partial class SceneManager : Node
{
    [Signal]
    public delegate void SceneChangeStartedEventHandler(string scenePath);

    [Signal]
    public delegate void SceneChangeCompletedEventHandler(string scenePath);

    [Signal]
    public delegate void LoadingProgressEventHandler(float progress);

    [Signal]
    public delegate void TransitionStartedEventHandler();

    [Signal]
    public delegate void TransitionFinishedEventHandler();

    private string currentScenePath;
    private Stack<string> sceneHistory = new();
    private Dictionary<string, PackedScene> transitionCache = [];
    private Dictionary<string, PackedScene> sceneCache = [];

    // FIXED SINGLETON - NO PRIVATE CONSTRUCTOR
    public static SceneManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            ProcessMode = ProcessModeEnum.Always; // CRITICAL FOR AUTOLOAD
            GD.Print("✅ SceneManager initialized");
        }
        else
        {
            GD.PrintErr("❌ Duplicate SceneManager - queuing free");
            QueueFree();
        }
    }

    // BASIC SCENE CHANGE - ADD NULL CHECK
    public void ChangeScene(string scenePath)
    {
        // CHECK IF WE HAVE A VALID SCENE TREE
        if (GetTree() == null)
        {
            GD.PrintErr("❌ SceneManager has no scene tree - cannot change scene");
            return;
        }

        if (!ResourceLoader.Exists(scenePath))
        {
            GD.PrintErr($"Scene not found: {scenePath}");
            return;
        }

        if (GetTree().CurrentScene != null)
        {
            sceneHistory.Push(currentScenePath);
        }

        GetTree().ChangeSceneToFile(scenePath);
        currentScenePath = scenePath;
    }

    // SCENE CHANGE WITH TRANSITION - ADD NULL CHECKS
    public async void ChangeSceneWithTransition(
        string scenePath,
        string transitionType = "fade",
        float duration = 0.5f
    )
    {
        // NULL CHECK SCENE TREE
        if (GetTree() == null)
        {
            GD.PrintErr("❌ SceneManager disposed - cannot change scene");
            GetTree()?.ChangeSceneToFile(scenePath); // Try direct change
            return;
        }

        if (!ResourceLoader.Exists(scenePath))
        {
            GD.PrintErr($"Scene not found: {scenePath}");
            return;
        }

        EmitSignal(SignalName.SceneChangeStarted, scenePath);
        EmitSignal(SignalName.TransitionStarted);

        // Save to history
        if (GetTree().CurrentScene != null)
        {
            sceneHistory.Push(currentScenePath);
        }

        // Play transition out
        await PlayTransition(transitionType, duration, true);

        // Change scene
        GetTree().ChangeSceneToFile(scenePath);
        currentScenePath = scenePath;

        // Play transition in
        await PlayTransition(transitionType, duration, false);

        EmitSignal(SignalName.TransitionFinished);
        EmitSignal(SignalName.SceneChangeCompleted, scenePath);
    }

    // PRIVATE METHODS WITH NULL CHECKS
    private async Task PlayTransition(string transitionType, float duration, bool isOut)
    {
        GD.Print($"🎬 PlayTransition called: {transitionType}, isOut: {isOut}");

        var transition = CreateTransition(transitionType);
        if (transition == null)
        {
            GD.PrintErr($"❌ Transition scene not found: {transitionType}");
            return;
        }

        GetTree().Root.AddChild(transition);
        GD.Print($"✅ Transition added to scene tree: {transition.Name}");

        // Set duration if transition supports it
        if (transition.HasMethod("SetDuration"))
        {
            transition.Call("SetDuration", duration);
            GD.Print($"✅ Duration set to: {duration}");
        }

        // Play transition
        string methodName = isOut ? "PlayOut" : "PlayIn";
        if (transition.HasMethod(methodName))
        {
            GD.Print($"🎬 Calling {methodName} on transition");
            transition.Call(methodName);
            await ToSignal(transition, "TransitionFinished");
            GD.Print($"🎬 Transition finished: {methodName}");
        }
        else
        {
            GD.PrintErr($"❌ Transition missing method: {methodName}");
        }

        transition.QueueFree();
        GD.Print("✅ Transition queued for free");
    }

    private Node CreateTransition(string transitionType)
    {
        // /Transition/FadeTransition.tscn
        string path =
            $"res://SceneManager/Transition/{transitionType.ToPascalCase()}Transition.tscn";
        GD.Print($"🔍 Looking for transition at: {path}");

        if (ResourceLoader.Exists(path))
        {
            var scene = GD.Load<PackedScene>(path);
            GD.Print($"✅ Transition scene found: {path}");
            return scene.Instantiate<Node>();
        }
        else
        {
            GD.PrintErr($"❌ Transition scene not found: {path}");
            return null;
        }
    }

    private Node CreateLoadingScreen(string loadingScreenType)
    {
        string path = $"res://Scenes/UI/{loadingScreenType}_loading.tscn";
        if (ResourceLoader.Exists(path))
        {
            var scene = GD.Load<PackedScene>(path);
            return scene.Instantiate<Node>();
        }
        return null;
    }

    // ASYNC LOADING WITH PROGRESS
    public async void LoadSceneAsync(string scenePath, string loadingScreenType = "default")
    {
        if (GetTree() == null)
            return;

        EmitSignal(SignalName.SceneChangeStarted, scenePath);

        // Show loading screen
        var loadingScreen = CreateLoadingScreen(loadingScreenType);
        GetTree().Root.AddChild(loadingScreen);

        // Simulate loading progress
        for (int i = 0; i <= 100; i++)
        {
            EmitSignal(SignalName.LoadingProgress, i / 100.0f);
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
        }

        // Remove loading screen
        loadingScreen.QueueFree();

        // Change scene
        GetTree().ChangeSceneToFile(scenePath);
        currentScenePath = scenePath;

        EmitSignal(SignalName.SceneChangeCompleted, scenePath);
    }

    // GO BACK TO PREVIOUS SCENE
    public void GoBack(string transitionType = "fade", float duration = 0.5f)
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            ChangeSceneWithTransition(previousScene, transitionType, duration);
        }
    }

    // PRELOAD SCENES
    public void PreloadScene(string scenePath)
    {
        if (ResourceLoader.Exists(scenePath) && !sceneCache.ContainsKey(scenePath))
        {
            sceneCache[scenePath] = GD.Load<PackedScene>(scenePath);
        }
    }

    // PUBLIC UTILITIES
    public void ClearHistory() => sceneHistory.Clear();

    public bool CanGoBack() => sceneHistory.Count > 0;

    public string GetCurrentScene() => currentScenePath;

    public int GetHistoryCount() => sceneHistory.Count;
}
