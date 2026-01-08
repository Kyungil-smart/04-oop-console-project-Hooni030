public struct Tile
{
    public GameObject OnTileObject { get; set; }

    public event Action OnStepPlayer;

    public Vector Position { get; set; }

    public bool HasGameObject => OnTileObject != null;

    public Tile(Vector position)
    {
        Position = position;
    }

    public void Print()
    {
        if (HasGameObject)
            OnTileObject.Symbol.Print();
        else
            ' '.Print();
    }
}