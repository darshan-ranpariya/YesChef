using UnityEngine;

namespace YesChef.Data
{
    public enum IngredientType
    {
        Vegetable,
        Cheese,
        Meat
    }

    public enum IngredientState
    {
        Raw,
        Prepped
    }

    [CreateAssetMenu(fileName = "New Ingredient", menuName = "YesChef/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [Header("Basic Info")] public string ingredientName;
        public IngredientType type;

        [Header("Gameplay Stats")] public int baseScore;
        public bool requiresPrep;
        public float prepTime;

        [Header("Visuals (Fallback)")] public Color rawColor = Color.white;

        public Color preppedColor = Color.green;
    }
}