using UnityEngine;
using YesChef.Core;
using YesChef.Data;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Fridge : MonoBehaviour, IInteractable
    {
        [Header("Fridge Settings")] [SerializeField]
        private IngredientData ingredientToDispense;

        [SerializeField] private Ingredient ingredientPrefab; // The base visual prefab

        public bool TryInteract(PlayerInteractor player)
        {
            // If the player is already holding something, the fridge does nothing
            if (player.HeldIngredient != null)
            {
                return false;
            }

            // Spawn a new ingredient and give it to the player
            var newIngredient = Instantiate(ingredientPrefab);
            newIngredient.Initialize(ingredientToDispense);

            player.SetHeldIngredient(newIngredient);

            Debug.Log($"Dispensed {ingredientToDispense.ingredientName} from Fridge.");
            return true;
        }
    }
}