public static class InputManager
{
    private static ConsoleKey _current;

    private static readonly ConsoleKey[] _Keys =
    {
        // 이동 : 상하좌우
        ConsoleKey.UpArrow,
        ConsoleKey.DownArrow,
        ConsoleKey.LeftArrow,
        ConsoleKey.RightArrow,

        // 공격
        ConsoleKey.Spacebar,

        // 메인 메뉴 복귀 화면 출력
        ConsoleKey.Escape, 

        ConsoleKey.Enter, // 선택

        ConsoleKey.L // 로그키
    };

    public static bool IsCorrectkey(ConsoleKey input)
        => _current == input;

    public static void Poll()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey input = Console.ReadKey(true).Key;

            foreach (ConsoleKey key in _Keys)
            {
                if (key == input)
                {
                    _current = key;
                    break;
                }
            }
        }
    }

    public static void ResetKey()
        => _current = ConsoleKey.None;
    public static ConsoleKey UsedKey()
    {
        ConsoleKey key = _current;
        _current = ConsoleKey.None;
        return key;
    }
}