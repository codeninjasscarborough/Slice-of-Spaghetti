using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Ingredient Card", fileName = "New Ingredient Card")]
    public class IngredientCardData : CardData
    {
        [SerializeField] IngredientTier tier;
        [SerializeField] IngredientTag tags;
        [SerializeField] float scoreMultiplierContribution = 1f;
        [SerializeField] int bonusFlatPoints;

        public IngredientTier Tier => tier;
        public IngredientTag Tags => tags;
        public float ScoreMultiplierContribution => scoreMultiplierContribution;
        public int BonusFlatPoints => bonusFlatPoints;
    }
}
