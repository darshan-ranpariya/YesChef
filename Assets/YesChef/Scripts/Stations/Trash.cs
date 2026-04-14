using UnityEngine;
using YesChef.Core;
using YesChef.Player;

namespace YesChef.Stations
{
    public class StationTrash : MonoBehaviour, IInteractable
    {
        public bool TryInteract(PlayerInteractor player)
        {
            if (player.HeldIngredient == null)
            {
                return false;
            }

            var itemToTrash = player.TakeHeldIngredient();
            Destroy(itemToTrash.gameObject);

            Debug.Log("Item gone");
            return true;
        }
    }
}