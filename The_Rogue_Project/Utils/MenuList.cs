public class MenuList
{
    private List<(string text, Action action)> _menus;

    private int _currentMenuIndex;

    private Ractangle _outLine;

    private int _maxLength;

    public int CurrentIndex { get => _currentMenuIndex; }

    public MenuList(params (string, Action)[] menuTexts)
    {
        if (menuTexts.Length == 0)
        {
            _menus = new List<(string, Action)>();
        }
        else
        {
            _menus = menuTexts.ToList();
        }

        for (int i = 0; i < _menus.Count; i++)
        {
            int textWidth = _menus[i].text.GetTextWidth();

            if (_maxLength < textWidth)
            {
                _maxLength = textWidth;
            }
        }
        _outLine = new Ractangle(width: _maxLength + 4, height: _menus.Count + 2);
    }

    public void Reset()
        => _currentMenuIndex = 0;

    public void Add(string text, Action action)
    {
        _menus.Add((text, action));

        int textWidth = text.GetTextWidth();
        if (_maxLength < textWidth)
        {
            _maxLength = textWidth;
        }

        _outLine.Width = _maxLength + 6;
        _outLine.Height++;
    }

    public void Remove()
    {
        _menus.RemoveAt(_currentMenuIndex);

        int max = 0;

        foreach ((string text, Action action) in _menus)
        {
            int textWidth = text.GetTextWidth();

            if (max < textWidth)
                max = textWidth;
        }

        if (_maxLength != max) _maxLength = max;

        _outLine.Width = _maxLength + 6;
        _outLine.Height--;
    }

    public void Select()
    {
        if (_menus.Count == 0) return;

        _menus[_currentMenuIndex].action?.Invoke();


        if (_menus.Count == 0)
            _currentMenuIndex = 0;
        else if (_currentMenuIndex >= _menus.Count)
            _currentMenuIndex = _menus.Count - 1;
        return;
    }

    public void SelectUp()
    {
        _currentMenuIndex--;

        if (_currentMenuIndex < 0)
            _currentMenuIndex = 0;

        if (_menus[_currentMenuIndex].action == null)
            _currentMenuIndex--;
    }

    public void SelectDown()
    {
        _currentMenuIndex++;


        if (_currentMenuIndex >= _menus.Count)
            _currentMenuIndex = _menus.Count - 1;

        if (_menus[_currentMenuIndex].action == null)
            _currentMenuIndex++;
    }

    public void Render(int x, int y)
    {
        _outLine.X = x;
        _outLine.Y = y;
        _outLine.Draw();

        for (int i = 0; i < _menus.Count; i++)
        {
            y++;
            Console.SetCursorPosition(x + 1, y);
            if (i == _currentMenuIndex)
            {
                "➤ ".Print(ConsoleColor.DarkBlue);
                _menus[i].text.Print(ConsoleColor.Black, ConsoleColor.Yellow);
            }
            else
            {
                Console.Write("  ");
                _menus[i].text.Print();
            }
        }
    }
}