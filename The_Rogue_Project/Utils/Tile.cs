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
        {
            "  ".Print(default, ConsoleColor.DarkGray);
            return;
        }
        if (OnTileObject is Wall)
            OnTileObject.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
        else if (OnTileObject is Bullet)
            OnTileObject.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
        else if (OnTileObject is Monster)
            OnTileObject.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
        else if (OnTileObject is ExpOrb)
            OnTileObject.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
        else
            OnTileObject.Symbol.Print(ConsoleColor.Black, ConsoleColor.DarkGray);
    }
}