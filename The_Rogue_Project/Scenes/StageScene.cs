using System.Numerics;
using System.Runtime.InteropServices;

public class StageScene : Scene
{
    private int _stageLevel;

    public static readonly int Field_Width = 13;
    public static readonly int Field_Height = 11;

    private Tile[,] _field = new Tile[Field_Height, Field_Width];
    private Ractangle TimeUI = new Ractangle(0, 0, 27, 7);

    public static Wall wall = new Wall();

    private int _second = 0;

    private PlayerCharacter _player;

    public StageScene(PlayerCharacter player, int level)
    {
        _stageLevel = level;
        Init(player);
    }

    public void Init(PlayerCharacter player)
    {
        _player = player;

        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _field[y, x] = new Tile(pos);
            }
        }
    }

    private void PlayerStatInit()
    {
        switch (_stageLevel)
        {
            case 0:
                _player.MaxHp = 7;
                _player.CurrentShield = 1;
                _player.MaxShield = 3;
                _player.BaseDamage = 2;
                break;
            case 1:
                _player.MaxHp = 5;
                _player.CurrentShield = 0;
                _player.MaxShield = 3;
                _player.BaseDamage = 1;
                break;
            case 2:
                _player.MaxHp = 3;
                _player.CurrentShield = 0;
                _player.MaxShield = 2;
                _player.BaseDamage = 1;
                break;
        }

        _player.StatInit();
    }

    public override void Enter()
    {
        PlayerStatInit();

        GameManager.Second = 0;


        // 벽 생성
        int wH = _field.GetLength(0);
        int wW = _field.GetLength(1);
        
        // 위, 아래 벽 
        for(int x = 0; x < wW; x++)
        {
            if (x == 6) continue;
            _field[0, x].OnTileObject = wall;
            _field[wH - 1, x].OnTileObject = wall;
        }

        // 좌, 우 벽
        for (int y = 0; y < wH; y++)
        {
            if(y == 5) continue;
            _field[y, 0].OnTileObject = wall;
            _field[y, wW - 1].OnTileObject = wall;
        }


        _player.Field = _field;
        _player.Position = new Vector(Field_Width/2, Field_Height/2);
        //_player.Position = new Vector(6,1);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

    }
    public override void Update()
    {
        _player.Update();
    }
    public override void Render()
    {
        PrintField(27, 7);
        _player.Render();
        PrintTimeUI(0, 0);
    }
    public override void Exit()
    {
        _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _player.Field = null;
    }
    private void PrintField(int pX, int pY)
    {

        for (int y = 0; y < _field.GetLength(0); y++)
        {
            Console.SetCursorPosition(pX, pY + y);

            for (int x = 0; x < _field.GetLength(1); x++)
            {
                if (_field[y, x].OnTileObject == _player)
                    _player.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
                else if (_field[y, x].OnTileObject is Wall)
                    "🧱".Print(ConsoleColor.Black, ConsoleColor.DarkGray);
                else
                    _field[y, x].Print();
            }
            Console.WriteLine();
        }

        if (GameManager.Second > 25)
        {
            _second++;
            GameManager.Second = 0;
        }
    }

    private void PrintTimeUI(int x, int y)
    {
        TimeUI.Draw(ConsoleColor.DarkCyan);

        Console.SetCursorPosition(x + 3, y + 1);
        $"난이도 : {_stageLevel + 1}단계".Print(ConsoleColor.Cyan);

        Console.SetCursorPosition(x + 2, y + 3);
        "🕒목표 시간 ⏳생존 시간".Print(ConsoleColor.Cyan);

        int purposeTime = 60 + (_stageLevel * 30);

        Console.SetCursorPosition(x + 7, y + 4);
        string timeString = $"{purposeTime}           {_second}";

        timeString.Print(ConsoleColor.Blue);
    }
}