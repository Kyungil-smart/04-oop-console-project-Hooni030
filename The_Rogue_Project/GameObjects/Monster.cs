public class Monster : GameObject
{
    public int Hp { get; private set; }
    public int Damage { get; private set; }

    public float StepInterval { get; private set; }
    public float StepTimer { get; set; }

    public bool IsDead => Hp <= 0;

    public Monster(int hp, int damage, float stepInterval)
    {
        Symbol = "😈";
        Hp = hp;
        Damage = damage;
        StepInterval = stepInterval;
        StepTimer = stepInterval;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }
}
