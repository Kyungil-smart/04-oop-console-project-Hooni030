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
        //_mainMenu.Add("", null);
        _mainMenu.Add("게임 종료", GameQuit);
    }
    public override void Enter()
    {
        _mainMenu.Reset();
    }
    public override void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.UpArrow))
            _mainMenu.SelectUp();
        if (InputManager.IsCorrectkey(ConsoleKey.DownArrow))
            _mainMenu.SelectDown();
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
            _mainMenu.Select();
    }
    public override void Render()
    {
        Console.SetCursorPosition(0, 2);
        GameManager.GameTitle.Print(ConsoleColor.Magenta);

        _mainMenu.Render(30, 15);
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