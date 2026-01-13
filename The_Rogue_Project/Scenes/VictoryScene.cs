public class VictoryScene : Scene
{
    private MenuList _victoryMenu;

    string victory = 
    "oooooo     oooo  o8o                .                                  \r\n" +
    " `888.     .8'   `\"'              .o8                                  \r\n" +
    "  `888.   .8'   oooo   .ooooo.  .o888oo   .ooooo.  oooo d8b oooo    ooo \r\n" +
    "   `888. .8'    `888  d88' `\"Y8   888    d88' `88b `888\"\"8P  `88.  .8'  \r\n" +
    "    `888.8'      888  888         888    888   888  888       `88..8'   \r\n" +
    "     `888'       888  888   .o8   888 .  888   888  888        `888'    \r\n" +
    "      `8'       o888o `Y8bod8P'   \"888\"  `Y8bod8P' d888b        .8'     \r\n" +
    "                                                            .o..P'      \r\n" +
    "                                                            `Y8P'       ";

    public VictoryScene() => Init();
    public void Init()
    {
        _victoryMenu = new MenuList();
        _victoryMenu.Add("초기 화면으로 돌아가기", MainMenu);
        _victoryMenu.Add("", null);
        _victoryMenu.Add("게임 종료", End);
    }
    public override void Enter()
    {
        _victoryMenu.Reset();
    }
    public override void Update()
    {
        ConsoleKey key = InputManager.UsedKey();
        if (key == ConsoleKey.None) return;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                _victoryMenu.SelectUp();
                break;
            case ConsoleKey.DownArrow:
                _victoryMenu.SelectDown();
                break;
            case ConsoleKey.Enter:
                _victoryMenu.Select();
                break;
        }
    }
    public override void Render()
    {
        Console.SetCursorPosition(0, 3);
        victory.Print(ConsoleColor.DarkYellow);

        _victoryMenu.Render(20, 15);
    }
    public override void Exit()
    {
    }
    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
    public void End()
    {
        Console.SetCursorPosition(23, 21);
        "게임을 종료합니다.".Print(ConsoleColor.Black, ConsoleColor.White);
        for(int i = 1; i <= 3; i++)
        {
            Thread.Sleep(500);
            Console.SetCursorPosition(40+i, 21);
            ".".Print(ConsoleColor.Black, ConsoleColor.White);
        }
        GameManager.isGameOver = true;
    }
}