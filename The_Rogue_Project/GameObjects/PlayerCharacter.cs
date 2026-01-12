using System.Runtime.InteropServices;

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> Level;
    public ObservableProperty<float> Exp;
    public ObservableProperty<float> HP;
    public Tile[,] Field { get; set; }

    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();

    private int stageWidth = StageScene.Field_Width;
    private int stageHeight = StageScene.Field_Height;

    private const int Stat_UI_Width = 27;
    private const int Stat_UI_Height = 11;
    private Ractangle StatUIWindow;

    private const int Level_UI_Width = 26;
    private const int Level_UI_Height = 7;
    private Ractangle LevelUIWindow;

    private string LevelIcon = "⭐";
    private int MaxExp;
    private float _expPercent;

    private string ExpIcon = "✨";
    private string ExpBar = "🟩";
    public int MaxHp { get; set; }
    private float _hpPercent;
    private string _hpBar = "🟥";

    public int BaseDamage { get; set; }
    private int _damage;
    private string _damageIcon = "🗡️";

    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }
    private string _ShieldBar = "🛡";


    private void Init()
    {
        Symbol = "⭐";
        IsActiveControl = true;

        _inventory = new Inventory(this);

        StatUIWindow = new Ractangle(0, 7, Stat_UI_Width, Stat_UI_Height);
        LevelUIWindow = new Ractangle(27, 0, Level_UI_Width, Level_UI_Height);

        Level = new ObservableProperty<int>(1);
        Exp = new ObservableProperty<float>(0);
        HP = new ObservableProperty<float>();

        Exp.AddListener(NextExp);
        Level.AddListener(NextLevel);
        HP.AddListener(SetHP);

        HP.Value = MaxHp;
    }

    public void StatInit()
    {
        MaxExp = 4;

        HP.Value = MaxHp;

        _damage = BaseDamage;
    }

    public void Update()
    {
        ConsoleKey key = InputManager.UsedKey();
        if (key == ConsoleKey.None) return;

        switch (key)
        {
            case ConsoleKey.I:
                HandleControl();
                break;

            case ConsoleKey.UpArrow:
                //Symbol = '▲';
                Move(Vector.Up);
                _inventory.SelectUp();
                break;

            case ConsoleKey.DownArrow:
                //Symbol = '▼';
                Move(Vector.Down);
                _inventory.SelectDown();
                break;

            case ConsoleKey.LeftArrow:
                //Symbol = '◀';
                Move(Vector.Left);
                break;

            case ConsoleKey.RightArrow:
                //Symbol = '▶';
                Move(Vector.Right);
                break;

            case ConsoleKey.Enter:
                _inventory.Select();
                break;
        }
    }

    public void HandleControl()
    {
        _inventory._isInventoryActive = !_inventory._isInventoryActive;
        IsActiveControl = !_inventory._isInventoryActive;
    }

    private void Move(Vector direction)
    {
        if (Field == null || !IsActiveControl) return;

        Vector current = Position;
        Vector nextPos = current + direction;



        // 1. 맵 바깥은 아닌지?
        if (nextPos.X < 1 || nextPos.X > StageScene.Field_Width - 2 ||
            nextPos.Y < 1 || nextPos.Y > StageScene.Field_Height - 2)
            return;

        // 2. 벽인지?
        if (Field[nextPos.Y, nextPos.X].OnTileObject is Wall)
            return;

        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;


        if (nextTileObject != null)
        {
            if (nextTileObject is IInteractable)
            {
                (nextTileObject as IInteractable)?.Interact(this);
            }
        }

        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;

        Position = nextPos;
    }

    public void Render()
    {
        StatUIWindow.Draw(ConsoleColor.Yellow);
        LevelUIWindow.Draw(ConsoleColor.DarkYellow);
        _inventory.Render();
        RenderPlayerUI();
    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }

    public void RenderPlayerUI()
    {
        RenderHP(2, 8);
        RenderShield(2, 11);
        RenderDamage(2, 14);
        RenderLevel(30, 1);
    }

    public void RenderHP(int x, int y)
    {
        string hpUI = "체력 : " + HP.Value.ToString() + " / " + MaxHp.ToString();
        Console.SetCursorPosition(x, y);
        hpUI.Print();
        Console.SetCursorPosition(x, y + 1);

        _hpPercent = HP.Value / MaxHp * 10;
        for (int i = 0; i < _hpPercent; i++)
        {
            _hpBar.Print();
        }
    }
    public void RenderShield(int x, int y)
    {
        string shieldUI = "방어막 : " + CurrentShield.ToString() + " / " + MaxShield.ToString();
        Console.SetCursorPosition(x, y);
        shieldUI.Print();
        Console.SetCursorPosition(x, y + 1);
        for (int i = 0; i < CurrentShield; i++)
        {
            _ShieldBar.Print();
        }
    }
    public void RenderDamage(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        string damageUI = "공격력 : " + _damage.ToString();
        damageUI.Print();

        Console.SetCursorPosition(x, y+1);
        for(int i = 0; i< _damage; i++)
        {
            _damageIcon.Print();
        }
    }

    public void SetHP(float HP)
    {
    }
    public void RenderLevel(int x, int y)
    {
        string levelUI = LevelIcon + " Level : " + Level.Value.ToString();

        Console.SetCursorPosition(x, y);
        levelUI.Print();

        string expUI = ExpIcon + " Exp" + " " + Exp.Value.ToString() + " / " + MaxExp.ToString();
        Console.SetCursorPosition(x, y + 2);
        expUI.Print();

        Console.SetCursorPosition(26, 4);
        for (int i = 0; i < _expPercent; i++)
        {
            ExpBar.Print();
        }
    }

    public void NextExp(float exp)
    {
        _expPercent = Exp.Value / MaxExp * 10;
        if (_expPercent >= 10)
        {
            Debug.Log($"{_expPercent}");
            Exp.Value = Exp.Value - MaxExp;
            Level.Value++;
        }
    }

    public void NextLevel(int level)
    {
        MaxExp = MaxExp/2 * level / 3 + 3;
        MaxHp += level/2 + 1;
        if (HP.Value + HP.Value / 3 < MaxHp)
            HP.Value += MaxHp / 3;
        else
            HP.Value = MaxHp;

        _damage = BaseDamage + level / 3;
    }


    //public void SetHealthGauge(int health)
    //{

    //}

    //public void Heal(int value)
    //{
    //    //HP.Value += value;
    //}
}