using UnityEngine;
using YesChef.Core;
using YesChef.Data;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Stove : MonoBehaviour, IInteractable
    {
        // Simple helper class to manage multiple slots on the stove
        [System.Serializable]
        private class StoveSlot
        {
            public Transform holdPoint;
            [HideInInspector] public Ingredient ingredient;
            [HideInInspector] public bool isCooking;
        }

        [Header("Stove Settings")] [SerializeField]
        private StoveSlot[] slots = new StoveSlot[2]; // Exactly 2 slots per requirements

        public bool TryInteract(PlayerInteractor player)
        {
            // 1. Prioritize taking a COOKED item if the player's hands are empty
            if (player.HeldIngredient == null)
            {
                foreach (var slot in slots)
                {
                    if (slot.ingredient != null && !slot.isCooking && slot.ingredient.CurrentState == IngredientState.Prepped)
                    {
                        player.SetHeldIngredient(slot.ingredient);
                        slot.ingredient = null;
                        return true;
                    }
                }
            }

            // 2. Prioritize placing RAW MEAT if player is holding it
            if (player.HeldIngredient != null && player.HeldIngredient.Data.type == IngredientType.Meat && player.HeldIngredient.CurrentState == IngredientState.Raw)
            {
                foreach (var slot in slots)
                {
                    if (slot.ingredient == null) // Find first empty slot
                    {
                        slot.ingredient = player.TakeHeldIngredient();
                        slot.ingredient.transform.SetParent(slot.holdPoint);
                        slot.ingredient.transform.localPosition = Vector3.zero;
                        slot.ingredient.transform.localRotation = Quaternion.identity;

                        CookMeatAsync(slot); // Fire-and-forget async timer
                        return true;
                    }
                }
            }

            return false; // Invalid interaction (e.g. stove is full, or player holds cheese)
        }

        private async void CookMeatAsync(StoveSlot slot)
        {
            slot.isCooking = true;
            var timer = 0f;
            var prepTime = slot.ingredient.Data.prepTime;

            Debug.Log($"Started cooking {slot.ingredient.Data.ingredientName}...");

            try
            {
                while (timer < prepTime)
                {
                    timer += Time.deltaTime;
                    // TODO: Update UI progress bar here in Phase 3

                    await Awaitable.NextFrameAsync(destroyCancellationToken);
                }

                slot.ingredient.SetState(IngredientState.Prepped);
                Debug.Log($"Finished cooking {slot.ingredient.Data.ingredientName}!");
            }
            catch (System.OperationCanceledException)
            {
                // Clean exit if play mode stops
            }
            finally
            {
                slot.isCooking = false;
            }
        }
    }
}