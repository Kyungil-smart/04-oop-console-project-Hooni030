using System.Diagnostics;

public class Time
{
    // 마지막 프레임의 경과 시간 저장 변수
    private static double _prevTime;
    private static double _currentTime;

    // 델타 타임(이전 프레임부터 현재 프레임까지 경과된 시간)과 총 경과 시간
    public static double DeltaTime { get; private set; }
    // 총 경과 시간 
    public static double TotalTime { get; private set; }

    // Stopwatch 인스턴스 생성
    private static Stopwatch _stopwatch;

    public Time() => Init();

    public static void Init()
    {
        _stopwatch = Stopwatch.StartNew();
        _prevTime = 0;
        DeltaTime = 0d;
        TotalTime = 0d;
    }

    public static void Update()
    {
        _currentTime = _stopwatch.ElapsedMilliseconds;
        double _deltaTime = _currentTime - _prevTime;
        _prevTime = _currentTime;

        DeltaTime = _deltaTime / 1000d;
        TotalTime = _currentTime / 1000d;
    }
}