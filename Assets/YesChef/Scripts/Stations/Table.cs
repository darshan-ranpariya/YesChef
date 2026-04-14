using System;
using UnityEngine;
using YesChef.Core;
using YesChef.Player;
using YesChef.Data;

namespace YesChef.Stations
{
    public class Table : MonoBehaviour, IInteractable
    {
        [Header("Table Settings")] [SerializeField]
        private Transform placementPoint;

        private Ingredient currentIngredient;
        private bool isProcessing;

        public bool TryInteract(PlayerInteractor player)
        {
            // 1. If player has RAW VEGETABLE and table is empty -> PLACE & CHOP
            if (currentIngredient == null && player.HeldIngredient != null)
            {
                if (player.HeldIngredient.Data.type == IngredientType.Vegetable && player.HeldIngredient.CurrentState == IngredientState.Raw)
                {
                    currentIngredient = player.TakeHeldIngredient();
                    currentIngredient.transform.SetParent(placementPoint);
                    currentIngredient.transform.localPosition = Vector3.zero;
                    currentIngredient.transform.localRotation = Quaternion.identity;

                    ProcessIngredientAsync(); // Fire-and-forget async timer
                    return true;
                }
            }

            // 2. If table has PREPPED VEGETABLE and player is empty -> TAKE
            if (currentIngredient != null && !isProcessing && player.HeldIngredient == null)
            {
                if (currentIngredient.CurrentState == IngredientState.Prepped)
                {
                    player.SetHeldIngredient(currentIngredient);
                    currentIngredient = null;
                    return true;
                }
            }

            return false; // Invalid interaction
        }

        private async void ProcessIngredientAsync()
        {
            isProcessing = true;
            var timer = 0f;
            var prepTime = currentIngredient.Data.prepTime;

            Debug.Log($"Started chopping {currentIngredient.Data.ingredientName}...");

            try
            {
                while (timer < prepTime)
                {
                    timer += Time.deltaTime;
                    // TODO: Update UI progress bar here in Phase 3

                    // Unity 6 safe await! Automatically cancels if the station is destroyed
                    await Awaitable.NextFrameAsync(destroyCancellationToken);
                }

                currentIngredient.SetState(IngredientState.Prepped);
                Debug.Log($"Finished chopping {currentIngredient.Data.ingredientName}!");
            }
            catch (OperationCanceledException)
            {
                // Station was destroyed or play mode stopped, exit cleanly without errors
            }
            finally
            {
                isProcessing = false;
            }
        }
    }
}