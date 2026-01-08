public class LogScene : Scene
{
    public override void Enter()
    {
    }
    public override void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
        {
            SceneManager.ChangePrevScene();
        }
    }
    public override void Render()
    {
        Debug.Render();
    }
    public override void Exit()
    {
    }
}