using System.Text;
using TMPro;
using UnityEngine;
using YesChef.Core;
using YesChef.Data;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Stove : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class StoveSlot
        {
            public Transform holdPoint;
            public TMP_Text timerText;
            [HideInInspector] public Ingredient ingredient;
            [HideInInspector] public bool isCooking;
        }

        [Header("Stove Settings")] [SerializeField]
        private StoveSlot[] slots = new StoveSlot[2];

        public bool TryInteract(PlayerInteractor player)
        {
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

            if (player.HeldIngredient != null && player.HeldIngredient.Data.type == IngredientType.Meat && player.HeldIngredient.CurrentState == IngredientState.Raw)
            {
                foreach (var slot in slots)
                {
                    if (slot.ingredient == null)
                    {
                        slot.ingredient = player.TakeHeldIngredient();
                        slot.ingredient.transform.SetParent(slot.holdPoint);
                        slot.ingredient.transform.localPosition = Vector3.zero;
                        slot.ingredient.transform.localRotation = Quaternion.identity;

                        CookMeatAsync(slot);
                        return true;
                    }
                }
            }

            return false;
        }

        private async void CookMeatAsync(StoveSlot slot)
        {
            slot.isCooking = true;
            var timer = 0f;
            var prepTime = slot.ingredient.Data.prepTime;
            var sb = new StringBuilder();

            Debug.Log($"cook {slot.ingredient.Data.ingredientName}.");

            while (timer < prepTime)
            {
                timer += Time.deltaTime;

                sb.AppendFormat("{0:0.0}s", prepTime - timer);
                slot.timerText.text = sb.ToString();
                sb.Clear();

                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            slot.ingredient.SetState(IngredientState.Prepped);
            Debug.Log($"{slot.ingredient.Data.ingredientName} done");
            slot.isCooking = false;
        }
    }
}