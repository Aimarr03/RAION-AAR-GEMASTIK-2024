public enum DelivarableType
{
    Poisoned,
    Mutated
}
public interface IDelivarable
{
    public DelivarableType GetDelivarableType();
    public int GetBounty();
    public void OnDeloading();

    public void OnDelivered();
}
