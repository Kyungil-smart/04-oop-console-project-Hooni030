using System.ComponentModel;
using System.ComponentModel.Design;

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> HP = new ObservableProperty<int>(5);
    public ObservableProperty<int> MP = new ObservableProperty<int>(3);
    public Tile[,] Field { get; set; }

    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }
    public PlayerCharacter() => Init();

    private string _healthGauge;
    private string _manaGauge;

    public void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;

        _inventory = new Inventory(this);

        HP.AddListener(SetHealthGauge);
        HP.Value = 5;
        _healthGauge = "❤️❤️❤️❤️❤️";

        MP.AddListener(SetManaGauge);
        _manaGauge = "🔵🔵🔵";
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
        if (InputManager.IsCorrectkey(ConsoleKey.F))
        {
            HP.Value--;
        }
        if (InputManager.IsCorrectkey(ConsoleKey.Spacebar))
        {
            Heal(2);
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
        DrawHealthGauge();
        DrawManaGauge();
        _inventory.Render();
    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }

    public void DrawHealthGauge()
    {
        Console.SetCursorPosition(40, 5);
        _healthGauge.Print();
    }

    public void SetHealthGauge(int health)
    {
        switch (HP.Value)
        {
            case 5:
                _healthGauge = "❤️❤️❤️❤️❤️";
                break;
            case 4:
                _healthGauge = "❤️❤️❤️❤️💔";
                break;
            case 3:
                _healthGauge = "❤️❤️❤️💔💔";
                break;
            case 2:
                _healthGauge = "❤️❤💔💔💔";
                break;
            case 1:
                _healthGauge = "❤️💔💔💔💔";
                break;
            default:
                _healthGauge = "💔💔💔💔💔";
                break;
        }
    }

    public void Heal(int value)
    {
        HP.Value += value;
    }

    public void DrawManaGauge()
    {
        Console.SetCursorPosition(40, 7);
        _manaGauge.Print();
    }

    public void SetManaGauge(int mana)
    {
        switch (HP.Value)
        {
            case 3:
                _healthGauge = "🔵🔵🔵";
                break;
            case 2:
                _healthGauge = "🔵🔵";
                break;
            case 1:
                _healthGauge = "🔵";
                break;
            default:
                _healthGauge = "";
                break;
        }
    }
}