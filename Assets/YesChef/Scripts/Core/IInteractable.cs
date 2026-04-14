using YesChef.Player;

namespace YesChef.Core
{
    public interface IInteractable
    {
        // Returns true if the interaction was successful
        bool TryInteract(PlayerInteractor player);
    }
}