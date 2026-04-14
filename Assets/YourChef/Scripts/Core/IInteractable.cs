public interface IInteractable
{
    // Returns true if the interaction was successful
    bool TryInteract(PlayerInteractor player);
}