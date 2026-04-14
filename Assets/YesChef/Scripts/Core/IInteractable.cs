using YesChef.Player;

namespace YesChef.Core
{
    public interface IInteractable
    {
        bool TryInteract(PlayerInteractor player);
    }
}