using UnityEngine;
using System.Collections.Generic;
using YesChef.Core;
using YesChef.Data;
using YesChef.Player;

namespace YesChef.Stations
{
    public class CustomerWindow : MonoBehaviour, IInteractable
    {
        public bool HasActiveOrder { get; private set; }
        public float TimeActive { get; private set; }

        // We use a List so we can easily remove ingredients as the player hands them in
        public List<IngredientData> RequiredIngredients { get; private set; } = new();

        private int totalBaseScore;

        private void Update()
        {
            if (HasActiveOrder)
            {
                TimeActive += Time.deltaTime;
            }
        }

        public void StartNewOrder(List<IngredientData> newOrder)
        {
            RequiredIngredients = new List<IngredientData>(newOrder);
            HasActiveOrder = true;
            TimeActive = 0f;
            totalBaseScore = 0;

            foreach (var item in newOrder)
            {
                totalBaseScore += item.baseScore;
            }

            Debug.Log($"New Order Started! Requires {newOrder.Count} items.");
        }

        public bool TryInteract(PlayerInteractor player)
        {
            if (!HasActiveOrder || player.HeldIngredient == null)
            {
                return false;
            }

            var heldData = player.HeldIngredient.Data;
            var heldState = player.HeldIngredient.CurrentState;

            // 1. Check if the ingredient needs to be prepped but isn't
            if (heldData.requiresPrep && heldState != IngredientState.Prepped)
            {
                Debug.Log($"{heldData.ingredientName} needs to be prepared first!");
                return false;
            }

            // 2. Check if the window actually needs this ingredient
            if (RequiredIngredients.Contains(heldData))
            {
                // Take the item, destroy it, and check it off the list
                RequiredIngredients.Remove(heldData);
                Destroy(player.TakeHeldIngredient().gameObject);

                Debug.Log($"Accepted {heldData.ingredientName}! {RequiredIngredients.Count} remaining.");

                // 3. Is the order fully complete?
                if (RequiredIngredients.Count == 0)
                {
                    CompleteOrder();
                }

                return true;
            }

            return false; // Player is holding something we don't want
        }

        private void CompleteOrder()
        {
            HasActiveOrder = false;
            // Shout out to the Event Bus that we finished!
            GameEvents.OnOrderCompleted?.Invoke(this, totalBaseScore, TimeActive);
        }
    }
}