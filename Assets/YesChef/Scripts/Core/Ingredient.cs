using UnityEngine;
using YesChef.Data;

namespace YesChef.Core
{
    public class Ingredient : MonoBehaviour
    {
        public IngredientData Data { get; private set; }
        public IngredientState CurrentState { get; private set; }

        [SerializeField] private MeshRenderer meshRenderer;

        public void Initialize(IngredientData data)
        {
            Data = data;
            SetState(IngredientState.Raw);
        }

        public void SetState(IngredientState newState)
        {
            CurrentState = newState;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (meshRenderer != null && Data != null)
            {
                meshRenderer.material.color = CurrentState == IngredientState.Raw ? Data.rawColor : Data.preppedColor;
            }
        }
    }
}