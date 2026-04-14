using UnityEngine;
using System.Collections.Generic;
using YesChef.Core;
using YesChef.Data;
using YesChef.Stations;

namespace YesChef.Managers
{
    public class OrderManager : MonoBehaviour
    {
        [Header("Data References")] [SerializeField]
        private IngredientData[] allAvailableIngredients;

        [Header("Scene References")] [SerializeField]
        private CustomerWindow[] customerWindows = new CustomerWindow[4];

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
            foreach (var window in customerWindows)
            {
                GenerateOrderForWindow(window);
            }
        }

        private void HandleOrderCompleted(CustomerWindow window, int baseScore, float timeActive)
        {
            var timePenalty = Mathf.FloorToInt(timeActive);
            var finalOrderScore = baseScore - timePenalty;

            currentTotalScore += finalOrderScore;

            GameEvents.OnScoreChanged?.Invoke(currentTotalScore);

            Debug.Log(
                $"Order Complete. Base score was {baseScore}, but with a time penalty of {timePenalty}, your net score is {finalOrderScore}. Total score now: {currentTotalScore}.");

            RespawnOrderAsync(window);
        }

        private async void RespawnOrderAsync(CustomerWindow window)
        {
            var timer = 0f;
            while (timer < 5f)
            {
                timer += Time.deltaTime;
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            GenerateOrderForWindow(window);
        }

        private void GenerateOrderForWindow(CustomerWindow window)
        {
            var ingredientCount = Random.value > 0.5f ? 2 : 3;
            var newOrder = new List<IngredientData>();

            for (var i = 0; i < ingredientCount; i++)
            {
                var randomIngredient = allAvailableIngredients[Random.Range(0, allAvailableIngredients.Length)];
                newOrder.Add(randomIngredient);
            }

            window.StartNewOrder(newOrder);
        }
    }
}