using UnityEngine;
using System.Collections.Generic;
using System.Text;
using TMPro;
using YesChef.Core;
using YesChef.Data;
using YesChef.Player;

namespace YesChef.Stations
{
    public class CustomerWindow : MonoBehaviour, IInteractable
    {
        [Header("World UI References")] [SerializeField]
        private TextMeshProUGUI orderListText;

        [SerializeField] private TextMeshProUGUI orderTimerText;
        [SerializeField] private TextMeshProUGUI scorePopupText;

        public bool HasActiveOrder { get; private set; }
        public float TimeActive { get; private set; }

        public List<IngredientData> RequiredIngredients { get; private set; } = new();

        private int totalBaseScore;
        private Vector3 defaultPopupLocalPos;
        private float _scoreDuration = 4f;

        private void Start()
        {
            if (scorePopupText != null)
            {
                defaultPopupLocalPos = scorePopupText.transform.localPosition;
                scorePopupText.text = "";
                scorePopupText.gameObject.SetActive(false);
            }

            orderListText.text = "Waiting for order...";
            orderTimerText.text = "";
        }

        private void Update()
        {
            if (HasActiveOrder)
            {
                TimeActive += Time.deltaTime;
                orderTimerText.text = $"{Mathf.FloorToInt(TimeActive)}s";
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

            UpdateOrderUI();
            Debug.Log($"New Order Requires {newOrder.Count}");
        }

        public bool TryInteract(PlayerInteractor player)
        {
            if (!HasActiveOrder || player.HeldIngredient == null)
            {
                return false;
            }

            var heldData = player.HeldIngredient.Data;
            var heldState = player.HeldIngredient.CurrentState;

            if (heldData.requiresPrep && heldState != IngredientState.Prepped)
            {
                Debug.Log($"prepere {heldData.ingredientName}");
                return false;
            }

            if (RequiredIngredients.Contains(heldData))
            {
                RequiredIngredients.Remove(heldData);
                Destroy(player.TakeHeldIngredient().gameObject);

                UpdateOrderUI();

                if (RequiredIngredients.Count == 0)
                {
                    CompleteOrder();
                }

                return true;
            }

            return false;
        }

        private void CompleteOrder()
        {
            HasActiveOrder = false;

            var timePenalty = Mathf.FloorToInt(TimeActive);
            var finalScore = totalBaseScore - timePenalty;

            ShowScorePopupAsync(finalScore);

            orderListText.text = "Order Complete!";
            orderTimerText.text = "";

            GameEvents.OnOrderCompleted?.Invoke(this, totalBaseScore, TimeActive);
        }

        private void UpdateOrderUI()
        {
            if (RequiredIngredients.Count == 0) return;

            var sb = new StringBuilder();

            var itemCounts = new Dictionary<string, int>();
            foreach (var item in RequiredIngredients)
            {
                if (itemCounts.ContainsKey(item.ingredientName))
                {
                    itemCounts[item.ingredientName]++;
                }
                else
                {
                    itemCounts[item.ingredientName] = 1;
                }
            }

            foreach (var kvp in itemCounts)
            {
                sb.AppendLine($"- {kvp.Key} (x{kvp.Value})");
            }

            orderListText.text = sb.ToString();
        }

        private async void ShowScorePopupAsync(int score)
        {
            scorePopupText.gameObject.SetActive(true);

            scorePopupText.text = score >= 0 ? $"<color=green>+{score}</color>" : $"<color=red>{score}</color>";

            var originalColor = scorePopupText.color;

            var duration = _scoreDuration;
            var timer = 0f;

            var startPos = defaultPopupLocalPos;
            var endPos = defaultPopupLocalPos + Vector3.up * 1.5f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                var progress = timer / duration;

                scorePopupText.transform.localPosition = Vector3.Lerp(startPos, endPos, progress);

                var fadeColor = scorePopupText.color;
                fadeColor.a = Mathf.Lerp(originalColor.a, 0f, progress);
                scorePopupText.color = fadeColor;

                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            if (scorePopupText != null)
            {
                scorePopupText.text = "";
                scorePopupText.color = originalColor;
                scorePopupText.transform.localPosition = defaultPopupLocalPos;
                scorePopupText.gameObject.SetActive(false);
            }
        }
    }
}