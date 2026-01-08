
public class GameManager
{
    public static bool isGameOver;

    public const string GameTitle = "|''||''| '||                 '||'                                       \r\n   ||     || ..     ....      ||         ...     ... . ... ...    ....  \r\n   ||     ||' ||  .|...||     ||       .|  '|.  || ||   ||  ||  .|...|| \r\n   ||     ||  ||  ||          ||       ||   ||   |''    ||  ||  ||      \r\n  .||.   .||. ||.  '|...'    .||.....|  '|..|'  '||||.  '|..'|.  '|...' \r\n                                               .|....'                  ";

    private PlayerCharacter _player;

    public void Run()
    {
        Init();

        while (!isGameOver)
        {
            Console.Clear();

            SceneManager.Render();

            InputManager.GetUserInput();

            if (InputManager.IsCorrectkey(ConsoleKey.L))
            {
                SceneManager.ChangeScene("Log");
            }

            SceneManager.Update();
        }
    }

    public void Init()
    {
        isGameOver = false;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        SceneManager.OnSceneChange += InputManager.ResetKey;

        //_player = new PlayerCharacter();

        //SceneManager.AddScene("MainMenu", new MainMenuScene());
        //SceneManager.AddScene("Stage", new StageScene(_player));
        //SceneManager.AddScene("StageSelect", new StageSelectScene());
        //SceneManager.AddScene("Guide", new GuideScene());
        //SceneManager.AddScene("Credit", new CreditScene());
        //SceneManager.AddScene("Log", new LogScene());

        SceneManager.ChangeScene("MainMenu");

        // 로그로 핀 포인트 체크
        Debug.Log("초기화 완료");
    }
}
