public static class InputManager
{
    private static ConsoleKey _current;

    private static readonly ConsoleKey[] _Keys =
    {
        ConsoleKey.UpArrow,
        ConsoleKey.DownArrow,
        ConsoleKey.LeftArrow,
        ConsoleKey.RightArrow,
        ConsoleKey.I, // 인벤토리
        ConsoleKey.L, // 로그키
        ConsoleKey.Enter // 선택
    };

    public static bool IsCorrectkey(ConsoleKey input)
        => _current == input;

    public static void GetUserInput()
    {
        ConsoleKey input = Console.ReadKey(true).Key;
        _current = ConsoleKey.None;

        foreach (ConsoleKey key in _Keys)
        {
            if (key == input)
            {
                _current = key;
                break;
            }
        }
    }
    public static void ResetKey()
        => _current = ConsoleKey.None;
}