using System.Runtime.InteropServices;

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> Level;
    public ObservableProperty<float> Exp;
    public Tile[,] Field { get; set; }

    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();

    private int stageWidth = StageScene.Field_Width;
    private int stageHeight = StageScene.Field_Height;

    private const int Stat_UI_Width = 18;
    private const int Stat_UI_Height = 10;
    private Ractangle StatUIWindow;

    private const int Level_UI_Width = 25;
    private const int Level_UI_Height = 7;
    private Ractangle LevelUIWindow;

    private string LevelIcon = "⭐";
    private int MaxExp { get; set; }
    private float expPercent {  get; set; }

    private string ExpIcon = "✨";
    private string ExpBar = "🟩";
    public int MaxHp { get; set; }
    private int _currentHp;
    private float _hpPercent;
    private string _hpBar = "🟥";

    public int BaseDamage { get; set; }
    private int _currentDamage;
    private string _damageIcon = "🗡️";

    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }

    private string _ShieldBar = "🛡";


    private void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;

        _inventory = new Inventory(this);

        StatUIWindow = new Ractangle(0, 7, Stat_UI_Width, Stat_UI_Height);
        LevelUIWindow = new Ractangle(19, 0, Level_UI_Width, Level_UI_Height);

        Exp = new ObservableProperty<float>(0);
        Level = new ObservableProperty<int>(1);

        Exp.AddListener(NextExp);
        Level.AddListener(NextLevel);
    }

    public void StatInit()
    {
        MaxExp = 3;

        _currentHp = MaxHp;

        _currentDamage = BaseDamage;
    }

    public void Update()
    {
        if (InputManager.IsCorrectkey(ConsoleKey.I))
            HandleControl();

        if (InputManager.IsCorrectkey(ConsoleKey.UpArrow))
        {
            Move(Vector.Up);
            _inventory.SelectUp();
        }
        if (InputManager.IsCorrectkey(ConsoleKey.DownArrow))
        {
            Move(Vector.Down);
            _inventory.SelectDown();
        }
        if (InputManager.IsCorrectkey(ConsoleKey.LeftArrow))
        {
            Move(Vector.Left);
        }
        if (InputManager.IsCorrectkey(ConsoleKey.RightArrow))
        {
            Move(Vector.Right);
        }
        if (InputManager.IsCorrectkey(ConsoleKey.Enter))
        {
            Exp.Value++;
            _inventory.Select();
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
        if (nextPos.X < 0 || nextPos.X > StageScene.Field_Width - 1 ||
            nextPos.Y < 0 || nextPos.Y > StageScene.Field_Height - 1)
            return;
        // 2. 벽인지?

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
        RenderLevel(22, 1);
    }

    public void RenderHP(int x, int y)
    {
        string hpUI = "체력 : " + _currentHp.ToString() + " / " + MaxHp.ToString();
        Console.SetCursorPosition(x, y);
        hpUI.Print();

        Console.SetCursorPosition(x, y + 1);
        for (int i = 0; i < _currentHp; i++)
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
        string damageUI = "공격력 : " + _currentDamage.ToString();
        damageUI.Print();

        Console.SetCursorPosition(x, y+1);
        for(int i = 0; i< _currentDamage; i++)
        {
            _damageIcon.Print();
        }
    }

    public void RenderLevel(int x, int y)
    {
        string levelUI = LevelIcon + " Level : " + Level.Value.ToString();
        Console.SetCursorPosition(x, y);
        levelUI.Print();
        string expUI = ExpIcon + " Exp" + " " + Exp.Value.ToString() + " / " + MaxExp.ToString();
        Console.SetCursorPosition(x, y + 2);
        expUI.Print();

        Console.SetCursorPosition(22, 4);
        for (int i = 0; i < expPercent; i++)
        {
            ExpBar.Print();
        }
    }

    public void NextExp(float exp)
    {
        expPercent = Exp.Value / MaxExp * 10;
        if (expPercent >= 10)
        {

            Debug.Log($"{expPercent}");
            Exp.Value = 0;
            Level.Value++;
        }
    }

    public void NextLevel(int level)
    {
        MaxExp = MaxExp/2 * level / 3 + 3;
    }


    //public void SetHealthGauge(int health)
    //{

    //}

    //public void Heal(int value)
    //{
    //    //HP.Value += value;
    //}
}