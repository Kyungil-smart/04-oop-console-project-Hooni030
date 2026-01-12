public class ExpOrb : GameObject, IInteractable
{
    public int ExpAmount { get; private set; }
    private Action<ExpOrb> _onCollected;

    public ExpOrb(int expAmount, Action<ExpOrb> onCollected)
    {
        Symbol = "💫";
        ExpAmount = expAmount;
        _onCollected = onCollected;
    }

    public void Interact(PlayerCharacter player)
    {
        player.Exp.Value += ExpAmount;
        _onCollected?.Invoke(this);
    }
}