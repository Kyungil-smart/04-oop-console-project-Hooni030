public class GuideScene : Scene
{
    private MenuList _guideMenu;

    private readonly string[] guide =
    {
        "게임명 : The Logue",
        "",
        "장르 : 로그라이크",
        "",
        "게임 목표 : ",
        "몬스터들을 처치하며 일정시간 버텨내면 승리!",
        "",
        "조작법",
        "이동 : 방향키",
        "공격 : 스페이스 바",
        "",
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
        for (int i = 0; i < guide.Length; i++)
        {
            Console.SetCursorPosition(15, 4 + i);
            guide[i].Print();
        }
        _guideMenu.Render(23, 18);
    }
    public override void Exit()
    {
    }

    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
}