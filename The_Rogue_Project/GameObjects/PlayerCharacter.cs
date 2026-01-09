public class PlayerCharacter : GameObject
{
    //public ObservableProperty<int> HP;
    public Tile[,] Field { get; set; }

    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();

    private const int Stat_UI_Width = 18;
    private const int Stat_UI_Height = 10;
    private Ractangle StatUIWindow;

    private const int Level_UI_Width = 25;
    private const int Level_UI_Height = 6;
    private Ractangle LevelUIWindow;

    private int Level { get; set; }
    public int Exp { get; set; }
    public int MaxHp { get; set; }
    private int _CurrentHp;
    private string _hpBar = "🟥";

    private float _BaseDamage;
    public float CurrentDamage { get; set; }

    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }

    private string _ShieldBar = "🛡";


    private void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;

        _inventory = new Inventory(this);

        StatUIWindow = new Ractangle(26, 0, Stat_UI_Width, Stat_UI_Height);
        LevelUIWindow = new Ractangle(0, 10, Level_UI_Width, Level_UI_Height);

        //HP = new ObservableProperty<int> { Value = _playerMaxHp };

        //HP.AddListener(SetHealthGauge);
    }

    public void StatInit()
    {
        _CurrentHp = MaxHp;

        _BaseDamage = 2;
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
        RenderPlayerHP(28, 1);
        RenderPlayerShield(28, 4);
        RenderPlayerDamage(28, 7);
    }

    public void RenderPlayerHP(int x, int y)
    {
        string hpUI = _CurrentHp.ToString() + " / " + MaxHp.ToString();
        Console.SetCursorPosition(x, y);
        hpUI.Print();

        Console.SetCursorPosition(x, y + 1);
        for (int i = 0; i < _CurrentHp; i++)
        {
            _hpBar.Print();
        }
    }
    public void RenderPlayerShield(int x, int y)
    {
        string shieldUI = CurrentShield.ToString() + " / " + MaxShield.ToString();
        Console.SetCursorPosition(x, y);
        shieldUI.Print();
        Console.SetCursorPosition(x, y + 1);
        for (int i = 0; i < CurrentShield; i++)
        {
            _ShieldBar.Print();
        }
    }
    public void RenderPlayerDamage(int x, int y)
    {
        string damageUI = "공격력 : " + CurrentDamage.ToString();

        Console.SetCursorPosition(x, y);
        damageUI.Print();
    }
    //public void SetHealthGauge(int health)
    //{

    //}

    //public void Heal(int value)
    //{
    //    //HP.Value += value;
    //}
}