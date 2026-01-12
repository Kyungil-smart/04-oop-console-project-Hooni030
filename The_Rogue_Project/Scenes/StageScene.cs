using System.Numerics;
using System.Runtime.InteropServices;

public class StageScene : Scene
{
    private int _stageLevel;

    public static readonly int Field_Width = 12;
    public static readonly int Field_Height = 10;

    private Tile[,] _field = new Tile[Field_Height, Field_Width];
    private Ractangle TimeUI = new Ractangle(0, 0, 24, 7);

    private Wall wall = new Wall();

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
        _player.wall = this.wall;

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

        _player.Field = _field;
        _player.Position = new Vector(Field_Width/2, Field_Height/2);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;


        for (int i = 0; i < _field.GetLength(1); i++)
        {
            _field[0, i].OnTileObject = wall;
        }
    }
    public override void Update()
    {
        _player.Update();
    }
    public override void Render()
    {
        PrintField(24, 7);
        TimeUI.Draw();
        _player.Render();
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
                else if (_field[y, x].OnTileObject == wall)
                    "🧱".Print(ConsoleColor.Black, ConsoleColor.DarkGray);
                else
                    _field[y, x].Print();
            }
            Console.WriteLine();
        }
        _second.ToString().Print();

        if (GameManager.Second > 25)
        {
            _second++;
            GameManager.Second = 0;
        }
    }
}