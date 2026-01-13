public class DeadScene : Scene
{
    private MenuList _deadMenu;

    string youDied =
        "oooooo   oooo                            oooooooooo.    o8o                  .o8  \r\n" +
        " `888.   .8'                             `888'   `Y8b   `\"'                 \"888  \r\n" +
        "  `888. .8'    .ooooo.  oooo  oooo        888      888 oooo   .ooooo.   .oooo888  \r\n" +
        "   `888.8'    d88' `88b `888  `888        888      888 `888  d88' `88b d88' `888  \r\n" +
        "    `888'     888   888  888   888        888      888  888  888ooo888 888   888  \r\n" +
        "     888      888   888  888   888        888     d88'  888  888    .o 888   888  \r\n" +
        "    o888o     `Y8bod8P'  `V88V\"V8P'      o888bood8P'   o888o `Y8bod8P' `Y8bod88P\" ";

    public DeadScene() => Init();
    public void Init()
    {
        _deadMenu = new MenuList();
        _deadMenu.Add("초기 화면으로 돌아가기", MainMenu);
        _deadMenu.Add("", null);
        _deadMenu.Add("게임 종료", End);
    }
    public override void Enter()
    {
        _deadMenu.Reset();
    }
    public override void Update()
    {
        ConsoleKey key = InputManager.UsedKey();
        if (key == ConsoleKey.None) return;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                _deadMenu.SelectUp();
                break;
            case ConsoleKey.DownArrow:
                _deadMenu.SelectDown();
                break;
            case ConsoleKey.Enter:
                _deadMenu.Select();
                break;
        }
    }
    public override void Render()
    {
        Console.SetCursorPosition(0, 3);
        youDied.Print(ConsoleColor.DarkRed);

        _deadMenu.Render(25, 15);
    }
    public override void Exit()
    {
    }
    public void MainMenu()
        => SceneManager.ChangeScene("MainMenu");
    public void End()
    {
        Console.SetCursorPosition(28, 21);
        "게임을 종료합니다.".Print(ConsoleColor.Black, ConsoleColor.White);
        for(int i = 1; i <= 3; i++)
        {
            Thread.Sleep(600);
            Console.SetCursorPosition(45 + i, 21);
            ".".Print(ConsoleColor.Black, ConsoleColor.White);
        }
        GameManager.isGameOver = true;
    }
}
