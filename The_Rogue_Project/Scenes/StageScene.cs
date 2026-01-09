using System.Numerics;
using System.Runtime.InteropServices;

public class StageScene : Scene
{
    private int _stageLevel;

    private Tile[,] _field = new Tile[10, 25];

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
                break;
            case 1:
                _player.MaxHp = 5;
                _player.CurrentShield = 0;
                _player.MaxShield = 3;
                break;
            case 2:
                _player.MaxHp = 3;
                _player.CurrentShield = 0;
                _player.MaxShield = 2;
                break;
        }

        _player.StatInit();
        Debug.Log($"{_player.MaxHp}");
    }

    public override void Enter()
    {
        PlayerStatInit();

        _player.Field = _field;
        _player.Position = new Vector(5, 2);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;
    }
    public override void Update()
    {
        _player.Update();
    }
    public override void Render()
    {
        PrintField();
        _player.Render();
    }
    public override void Exit()
    {
        _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _player.Field = null;
    }
    private void PrintField()
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                if (_field[y, x].OnTileObject == _player)
                {
                    _player.Symbol.Print(ConsoleColor.Black, ConsoleColor.Gray);
                }
                else
                    _field[y, x].Print();
            }
            Console.WriteLine();
        }
    }
}