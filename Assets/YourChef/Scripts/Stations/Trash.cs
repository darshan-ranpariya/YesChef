using UnityEngine;

public class StationTrash : MonoBehaviour, IInteractable
{
    public bool TryInteract(PlayerInteractor player)
    {
        // If player has nothing, nothing to trash
        if (player.HeldIngredient == null)
        {
            return false;
        }

        // Take the item from the player and destroy it
        var itemToTrash = player.TakeHeldIngredient();
        Destroy(itemToTrash.gameObject);

        Debug.Log("Item trashed.");
        return true;
    }
}