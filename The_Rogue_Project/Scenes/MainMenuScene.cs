public class MainMenuScene : Scene
{
    private MenuList _mainMenu;

    public MainMenuScene() => Init();

    public void Init()
    {
        _mainMenu = new MenuList();
        _mainMenu.Add("게임 시작", GameStart);
        _mainMenu.Add("도움말", GameGuide);
        _mainMenu.Add("크레딧", Credit);
        _mainMenu.Add("", null);
        _mainMenu.Add("게임 종료", GameQuit);
    }
    public override void Enter()
    {
        _mainMenu.Reset();
    }
    public override void Update()
    {
        ConsoleKey key = InputManager.UsedKey();
        if (key == ConsoleKey.None) return;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                _mainMenu.SelectUp();
                break;
            case ConsoleKey.DownArrow:
                _mainMenu.SelectDown();
                break;
            case ConsoleKey.Enter:
                _mainMenu.Select();
                break;
        }
    }
    public override void Render()
    {
        Console.SetCursorPosition(0, 4);
        GameManager.GameTitle.Print(ConsoleColor.Magenta);

        _mainMenu.Render(28, 15);
    }
    public override void Exit()
    {
    }
    public void GameStart()
        => SceneManager.ChangeScene("StageSelect");
    public void GameGuide()
        => SceneManager.ChangeScene("Guide");
    public void Credit()
        => SceneManager.ChangeScene("Credit");
    public void GameQuit()
        => GameManager.isGameOver = true;
}