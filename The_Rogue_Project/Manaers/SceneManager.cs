public static class SceneManager
{
    public static Action OnSceneChange;

    public static Scene Current { get; private set; }

    private static Scene _prev;

    private static Dictionary<string, Scene> _scene = new Dictionary<string, Scene>();

    public static void AddScene(string Key, Scene scene)
    {
        if (_scene.ContainsKey(Key)) return;

        _scene.Add(Key, scene);
    }

    public static void ChangePrevScene()
    {
        ChangeScene(_prev);
    }

    public static void ChangeScene(string key)
    {
        if (!_scene.ContainsKey(key)) return;
        ChangeScene(_scene[key]);
    }

    public static void ChangeScene(Scene scene)
    {
        Scene next = scene;

        if (Current == next) return;

        Current?.Exit();
        next.Enter();

        _prev = Current;
        Current = next;
        OnSceneChange?.Invoke();
    }

    public static void Update()
        => Current?.Update();

    public static void Render()
        => Current?.Render();

    public static void Remove()
        => Current = null;
}