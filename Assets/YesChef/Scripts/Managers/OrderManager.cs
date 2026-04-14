using UnityEngine;
using System.Collections.Generic;
using YesChef.Core;
using YesChef.Data;
using YesChef.Stations;

namespace YesChef.Managers
{
    /// <summary>
    /// Handles generating random orders and calculating the global score.
    /// </summary>
    public class OrderManager : MonoBehaviour
    {
        [Header("Data References")] [SerializeField]
        private IngredientData[] allAvailableIngredients;

        [Header("Scene References")] [SerializeField]
        private CustomerWindow[] customerWindows = new CustomerWindow[4]; // Exactly 4 per requirements

        private int currentTotalScore = 0;

        private void OnEnable()
        {
            GameEvents.OnOrderCompleted += HandleOrderCompleted;
        }

        private void OnDisable()
        {
            GameEvents.OnOrderCompleted -= HandleOrderCompleted;
        }

        private void Start()
        {
            // At the start of the game, populate all 4 windows
            foreach (var window in customerWindows)
            {
                GenerateOrderForWindow(window);
            }
        }

        private void HandleOrderCompleted(CustomerWindow window, int baseScore, float timeActive)
        {
            // Math Formula from PDF: Base Score - Floor(Seconds Passed)
            var timePenalty = Mathf.FloorToInt(timeActive);
            var finalOrderScore = baseScore - timePenalty;

            currentTotalScore += finalOrderScore;

            // Tell the UI the score changed
            GameEvents.OnScoreChanged?.Invoke(currentTotalScore);

            Debug.Log($"<color=green>Order Complete!</color> Base: {baseScore}, Penalty: -{timePenalty}, Net: {finalOrderScore}. Total Score: {currentTotalScore}");

            // Start the 5-second respawn timer
            RespawnOrderAsync(window);
        }

        private async void RespawnOrderAsync(CustomerWindow window)
        {
            try
            {
                var timer = 0f;
                while (timer < 5f)
                {
                    timer += Time.deltaTime;
                    await Awaitable.NextFrameAsync(destroyCancellationToken);
                }

                GenerateOrderForWindow(window);
            }
            catch (System.OperationCanceledException)
            {
                // Manager was destroyed, clean exit
            }
        }

        private void GenerateOrderForWindow(CustomerWindow window)
        {
            // 50% chance for 2 items, 50% chance for 3 items
            var ingredientCount = Random.value > 0.5f ? 2 : 3;
            var newOrder = new List<IngredientData>();

            for (var i = 0; i < ingredientCount; i++)
            {
                // Randomly pick an ingredient from our array (Allows for duplicates automatically!)
                var randomIngredient = allAvailableIngredients[Random.Range(0, allAvailableIngredients.Length)];
                newOrder.Add(randomIngredient);
            }

            window.StartNewOrder(newOrder);
        }
    }
}