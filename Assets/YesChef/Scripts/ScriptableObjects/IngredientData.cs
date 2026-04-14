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

    /// <summary>
    /// ScriptableObject defining the core properties of our ingredients.
    /// This allows designers to tweak scores and times without touching code.
    /// </summary>
    [CreateAssetMenu(fileName = "New Ingredient", menuName = "YesChef/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [Header("Basic Info")] public string ingredientName;
        public IngredientType type;

        [Header("Gameplay Stats")] public int baseScore;
        public bool requiresPrep;
        public float prepTime;

        [Header("Visuals (Fallback)")] [Tooltip("Used to change the color of our primitive shapes based on state.")]
        public Color rawColor = Color.white;

        public Color preppedColor = Color.green;
    }
}