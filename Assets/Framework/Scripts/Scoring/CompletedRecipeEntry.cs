using System;
using System.Collections.Generic;

namespace CardGame
{
    public readonly struct ScoreBreakdown
    {
        public readonly int BaseIngredientPoints;
        public readonly int BonusFlatPoints;
        public readonly float IngredientMultiplier;
        public readonly int TierBonus;
        public readonly float TierMixBonus;
        public readonly int Total;

        public ScoreBreakdown(int baseIngredient, int bonusFlat, float ingredientMultiplier,
                              int tierBonus, float tierMixBonus)
        {
            BaseIngredientPoints = baseIngredient;
            BonusFlatPoints = bonusFlat;
            IngredientMultiplier = ingredientMultiplier;
            TierBonus = tierBonus;
            TierMixBonus = tierMixBonus;
            Total = (int)(((baseIngredient + bonusFlat) * ingredientMultiplier) + tierBonus) +
                    (int)tierMixBonus;
        }
    }

    public class CompletedRecipeEntry
    {
        public RecipeCardData Recipe { get; }
        public IReadOnlyList<Card> CardsUsed { get; }
        public ScoreBreakdown Breakdown { get; }
        public DateTime CompletedAt { get; }
        public bool WasTierMixed { get; }
        public PlayerSide Side { get; }

        public CompletedRecipeEntry(RecipeCardData recipe, List<Card> cardsUsed,
                                    ScoreBreakdown breakdown, bool wasTierMixed, PlayerSide side)
        {
            Recipe = recipe;
            CardsUsed = cardsUsed.AsReadOnly();
            Breakdown = breakdown;
            CompletedAt = DateTime.UtcNow;
            WasTierMixed = wasTierMixed;
            Side = side;
        }
    }
}
