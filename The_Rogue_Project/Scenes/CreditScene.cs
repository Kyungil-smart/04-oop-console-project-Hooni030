public class CreditScene : Scene
{
    private MenuList _sceneMenu;
    private readonly string Developer = "개발자 : 박승훈 (KGA 4기)";
    private readonly string[] Helper = { "도움 주신분", "강재성 강사님", "최영민 강사님", "이태호 매니저님" };

    public CreditScene() => Init();
    public void Init()
    {
        _sceneMenu = new MenuList();
        _sceneMenu.Add("초기 화면", MainMenu);
    }

    public override void Enter()
    {
        _sceneMenu.Reset();
    }
    public override void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
            _sceneMenu.Select();
    }
    public override void Render()
    {
        Console.SetCursorPosition(7, 2);
        Developer.Print(ConsoleColor.Green);
        Thread.Sleep(200);
        Console.WriteLine();
        for (int i = 0; i < 4; i++)
        {
            Thread.Sleep(300);

            Console.SetCursorPosition(12, 4 + i);

            Helper[i].Print(ConsoleColor.DarkYellow);
            Console.WriteLine();
        }
        Thread.Sleep(300);

        _sceneMenu.Render(13, 10);
    }
    public override void Exit()
    {
    }
    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
}