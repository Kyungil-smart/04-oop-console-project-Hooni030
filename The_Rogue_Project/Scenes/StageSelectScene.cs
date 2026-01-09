public class StageSelectScene : Scene
{
    private MenuList _stageList;
    public StageSelectScene() => Init();
    public void Init()
    {
        _stageList = new MenuList();
        _stageList.Add("쉬움", EasyStage);
        _stageList.Add("보통", NormalStage);
        _stageList.Add("어려움", HardStage);
        _stageList.Add("", null);
        _stageList.Add("초기 화면", MainMenu);
    }
    public override void Enter()
    {
        _stageList.Reset();
    }
    public override void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.UpArrow))
            _stageList.SelectUp();
        if (InputManager.IsCorrectkey(ConsoleKey.DownArrow))
            _stageList.SelectDown();
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
            _stageList.Select();
    }
    public override void Render()
    {
        Console.SetCursorPosition(17, 3);
        "난이도 선택".Print(ConsoleColor.DarkMagenta);

        _stageList.Render(15, 5);
    }
    public override void Exit()
    {
    }

    public void EasyStage()
    {
        SceneManager.ChangeScene("Easy");
    }
    public void NormalStage()
    {
        SceneManager.ChangeScene("Normal");
    }
    public void HardStage()
    {
        SceneManager.ChangeScene("Hard");
    }
    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
}