public class GuideScene : Scene
{
    private MenuList _guideMenu;

    private readonly string[] guide =
    {
        "조작법",
        "이동 : 방향키",
        "인벤토리 : I",
        "일시정지 : Space Bar",
        "메인 메뉴 : Escape"
    };

    public GuideScene() => Init();
    public void Init()
    {
        _guideMenu = new MenuList();
        _guideMenu.Add("초기 화면", MainMenu);
    }
    public override void Enter()
    {
        _guideMenu.Reset();
    }
    public override void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
            _guideMenu.Select();
    }
    public override void Render()
    {
        Console.SetCursorPosition(13, 2);
        guide[0].Print(ConsoleColor.DarkCyan);
        for (int i = 1; i < guide.Length; i++)
        {
            Console.SetCursorPosition(9, 3 + i);
            guide[i].Print();
        }
        _guideMenu.Render(10, 9);
    }
    public override void Exit()
    {
    }

    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
}