using System;
using System.Text;
using TMPro;
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

        [SerializeField] private TMP_Text timerTxt;

        private Ingredient currentIngredient;
        private bool isProcessing;

        public bool TryInteract(PlayerInteractor player)
        {
            if (currentIngredient == null && player.HeldIngredient != null)
            {
                if (player.HeldIngredient.Data.type == IngredientType.Vegetable && player.HeldIngredient.CurrentState == IngredientState.Raw)
                {
                    currentIngredient = player.TakeHeldIngredient();
                    currentIngredient.transform.SetParent(placementPoint);
                    currentIngredient.transform.localPosition = Vector3.zero;
                    currentIngredient.transform.localRotation = Quaternion.identity;

                    ProcessIngredientAsync();
                    return true;
                }
            }

            if (currentIngredient != null && !isProcessing && player.HeldIngredient == null)
            {
                if (currentIngredient.CurrentState == IngredientState.Prepped)
                {
                    player.SetHeldIngredient(currentIngredient);
                    currentIngredient = null;
                    return true;
                }
            }

            return false;
        }

        private async void ProcessIngredientAsync()
        {
            isProcessing = true;
            var timer = 0f;
            var prepTime = currentIngredient.Data.prepTime;
            var sb = new StringBuilder();
            Debug.Log($"chopping {currentIngredient.Data.ingredientName}");

            while (timer < prepTime)
            {
                timer += Time.deltaTime;
                sb.AppendFormat("{0:0.0}s", prepTime - timer);
                timerTxt.text = sb.ToString();
                sb.Clear();

                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            currentIngredient.SetState(IngredientState.Prepped);
            Debug.Log($"done {currentIngredient.Data.ingredientName} is ready.");

            isProcessing = false;
        }
    }
}