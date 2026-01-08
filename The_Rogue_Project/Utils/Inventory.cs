public class Inventory
{
    private List<Item> _items = new List<Item>();

    public bool _isInventoryActive { get; set; } = false;

    public MenuList _itemMenu = new MenuList();

    private PlayerCharacter _owner;

    public Inventory(PlayerCharacter onwer)
    {
        _owner = onwer;
    }

    public void Add(Item item)
    {
        Debug.Log("아이템 추가");

        if (_items.Count > 10) return;

        _items.Add(item);
        _itemMenu.Add(item.Name, item.Use);
        // _itemMenu.Add(item.Name, null);
        item._inventory = this;
        item.Owner = _owner;
    }

    public void Remove(Item item)
    {
        Debug.Log("아이템 삭제");
        _items.Remove(item);
        _itemMenu.Remove();
    }

    public void Render()
    {
        if (!_isInventoryActive) return;
        _itemMenu.Render(15, 1);
    }

    public void Select()
    {
        if (!_isInventoryActive) return;
        _itemMenu.Select();
    }
    public void SelectUp()
    {
        if (!_isInventoryActive) return;
        _itemMenu.SelectUp();
    }
    public void SelectDown()
    {
        if (!_isInventoryActive) return;
        _itemMenu.SelectDown();
    }
}