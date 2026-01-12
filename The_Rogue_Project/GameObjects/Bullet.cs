public class Bullet : GameObject
{
    public Vector Direction { get; private set; }
    public int Damage { get; private set; }

    public float FireInterval { get; private set; }
    public float FireTimer { get; set; }

    public Bullet(Vector start, Vector direction, int damage, float stepInterval)
    {
        Symbol = "•";
        Position = start;
        Direction = direction;
        Damage = damage;
        FireInterval = stepInterval;
        FireTimer = 0f;
    }
}