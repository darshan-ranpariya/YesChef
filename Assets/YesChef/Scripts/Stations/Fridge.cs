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

        [SerializeField] private Ingredient ingredientPrefab;

        public bool TryInteract(PlayerInteractor player)
        {
            if (player.HeldIngredient != null)
            {
                return false;
            }

            var newIngredient = Instantiate(ingredientPrefab);
            newIngredient.Initialize(ingredientToDispense);

            player.SetHeldIngredient(newIngredient);

            Debug.Log($"Got {ingredientToDispense.ingredientName}");
            return true;
        }
    }
}