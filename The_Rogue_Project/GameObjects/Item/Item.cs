public abstract class Item : GameObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Inventory _inventory { get; set; }
    public bool inInventory { get => _inventory != null; }
    public PlayerCharacter Owner { get; set; }


    public abstract void Use();

    public abstract void PrintInfo();
}