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
        ConsoleKey key = InputManager.UsedKey();
        if (key == ConsoleKey.None) return;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                _stageList.SelectUp();
                break;
            case ConsoleKey.DownArrow:
                _stageList.SelectDown();
                break;
            case ConsoleKey.Enter:
                _stageList.Select();
                break;
        }
    }
    public override void Render()
    {
        Console.SetCursorPosition(22, 6);
        "난이도 선택".Print(ConsoleColor.DarkRed);

        _stageList.Render(20, 9);
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