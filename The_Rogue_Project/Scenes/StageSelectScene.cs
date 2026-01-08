public class StageSelectScene : Scene
{
    private MenuList _stageList;
    public StageSelectScene() => Init();
    public void Init()
    {
        _stageList = new MenuList();
        _stageList.Add("쉬움", SelectStage);
        _stageList.Add("보통", SelectStage);
        _stageList.Add("어려움", SelectStage);
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
        Console.SetCursorPosition(11, 3);
        "난이도 선택".Print(ConsoleColor.DarkMagenta);

        _stageList.Render(12, 5);
    }
    public override void Exit()
    {
    }

    public void SelectStage()
    {
        SceneManager.ChangeScene("Stage");
    }
    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
}