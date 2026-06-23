using System;
using UnityEngine;

namespace CardGame
{
    [Serializable]
    public class RecipeRequirement
    {
        [SerializeField] RequirementMatchMode matchMode;

        // Used when matchMode == ExactCard
        [SerializeField] CardData exactCard;

        // Used when matchMode == AnyInTag
        [SerializeField] IngredientTag requiredTags;

        // Used when matchMode == AnyOfTier or AnyOfTierOrHigher
        [SerializeField] IngredientTier requiredTier;

        public RequirementMatchMode MatchMode => matchMode;
        public CardData ExactCard => exactCard;
        public IngredientTag RequiredTags => requiredTags;
        public IngredientTier RequiredTier => requiredTier;

        public bool IsSatisfiedBy(CardData card)
        {
            if (card == null) return false;

            return matchMode switch
            {
                RequirementMatchMode.ExactCard =>
                    card == exactCard,

                RequirementMatchMode.AnyInTag =>
                    card is IngredientCardData ing && (ing.Tags & requiredTags) != 0,

                RequirementMatchMode.AnyOfTier =>
                    card is IngredientCardData tierCard && tierCard.Tier == requiredTier,

                RequirementMatchMode.AnyOfTierOrHigher =>
                    card is IngredientCardData tierCard2 && (int)tierCard2.Tier >= (int)requiredTier,

                RequirementMatchMode.AnyRecipe =>
                    card is RecipeCardData,

                RequirementMatchMode.AnyUtility =>
                    card is UtilityCardData,

                _ => false
            };
        }
    }
}
