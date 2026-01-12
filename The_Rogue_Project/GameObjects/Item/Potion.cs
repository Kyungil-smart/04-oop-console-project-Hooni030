//using System.Buffers;

//public class Potion : Item, IInteractable
//{
//    public Potion() => Init();
//    private void Init()
//    {
//        Symbol = 'I';
//    }

//    public override void Use()
//    {
//        //Owner.Heal(1);

//        _inventory.Remove(this);
//        _inventory = null;
//        Owner = null;

//    }


//    public override void PrintInfo()
//    {
//    }

//    public void Interact(PlayerCharacter player)
//    {
//        player.AddItem(this);
//    }
//}