
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public class GameManager
{
    // 게임을 종료했는지 판단 하는 함수
    public static bool isGameOver { get; set; }

    // 게임 제목 ASCII 코드
    public const string GameTitle = "|''||''| '||                 '||'                                       \r\n" +
                                    "   ||     || ..     ....      ||         ...     ... . ... ...    ....  \r\n" +
                                    "   ||     ||' ||  .|...||     ||       .|  '|.  || ||   ||  ||  .|...|| \r\n" +
                                    "   ||     ||  ||  ||          ||       ||   ||   |''    ||  ||  ||      \r\n" +
                                    "  .||.   .||. ||.  '|...'    .||.....|  '|..|'  '||||.  '|..'|.  '|...' \r\n" +
                                    "                                               .|....'                  ";
    // 스테이지에서 관리할 플레이어 객체 선언
    private PlayerCharacter _player;

    Stopwatch _frame = Stopwatch.StartNew();
    const int _targetFrame = 25;
    const int targetFrameMs = 1000 / _targetFrame;

    // 실제 프로그램 구동 메서드
    public void Run()
    {
        // 초기화 함수 사용
        Init();
        // 시간 측정을 위한 스톱워치 시작
        Stopwatch watch = Stopwatch.StartNew();

        // 게임오버가 참일 때 프로그램 종료
        while (!isGameOver)
        {
            double frameStart = watch.ElapsedMilliseconds;

            Time.Update();
            // 다음 프레임 출력
            SceneManager.Render();

            // 콘솔 입력 받기
            InputManager.Poll();

            // 프로그램 중간에 L 키를 누르면 디버그(로그) 화면으로 진입
            if (InputManager.IsCorrectkey(ConsoleKey.L))
                SceneManager.ChangeScene("Log");

            // 상태 업데이트
            SceneManager.Update();

            double frameTime = watch.ElapsedMilliseconds - frameStart;
            int sleep = targetFrameMs - (int)frameTime;
            if (sleep > 0)
            {
                Thread.Sleep(sleep);

            }

        }
    }

    // 초기화 메서드
    public void Init()
    {
        // 기본 동작을 위해 false로 초기화
        isGameOver = false;

        // 콘솔 창 크기 및 버퍼 크기 설정
        Console.SetWindowSize(100, 50);
        Console.SetBufferSize(100, 50);

        // 특수문자(이모지 등) 사용을 위해 인코딩 변경
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // 콘솔 화면에 커서표시 비활성화
        Console.CursorVisible = false;

        // 씬이 변경될 때 마다 입력 키 None으로 초기화
        SceneManager.OnSceneChange += () =>
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        };
        SceneManager.OnSceneChange += InputManager.ResetKey;

        // 메인메뉴, 스테이지 선택, 도움말, 크레딧 씬 추가
        SceneManager.AddScene("MainMenu", new MainMenuScene());
        SceneManager.AddScene("Guide", new GuideScene());
        SceneManager.AddScene("Credit", new CreditScene());
        SceneManager.AddScene("StageSelect", new StageSelectScene());

        // 플레이어 객체 할당
        _player = new PlayerCharacter();

        // 스테이지 난이도 별로 객체 생성 후 씬 추가
        StageScene easy = new StageScene(_player, 0);
        StageScene normal = new StageScene(_player, 1);
        StageScene hard = new StageScene(_player, 2);

        SceneManager.AddScene("Easy", easy);
        SceneManager.AddScene("Normal", normal);
        SceneManager.AddScene("Hard", hard);

        // 로그 씬 추가
        SceneManager.AddScene("Log", new LogScene());

        // 초기 화면 메인 메뉴 씬으로 변경
        SceneManager.ChangeScene("MainMenu");

        // 로그로 핀 포인트 체크
        Debug.Log("초기화 완료");
    }
}
