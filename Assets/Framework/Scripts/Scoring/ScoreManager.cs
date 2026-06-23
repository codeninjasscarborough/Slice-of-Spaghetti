using System.Collections.Generic;

namespace CardGame
{
    public class PlayerScoreLedger
    {
        public int Total { get; private set; }
        public List<CompletedRecipeEntry> History { get; } = new();

        public void Add(CompletedRecipeEntry entry)
        {
            Total += entry.Breakdown.Total;
            History.Add(entry);
        }
    }

    public class ScoreManager
    {
        readonly Dictionary<PlayerSide, PlayerScoreLedger> _ledgers = new()
        {
            { PlayerSide.Local,    new PlayerScoreLedger() },
            { PlayerSide.Opponent, new PlayerScoreLedger() }
        };

        // Tier bonuses indexed by RecipeTier: Easy, Normal, Medium, Hard, SuperHard, Insane
        static readonly int[] TierBonuses = { 0, 5, 15, 30, 50, 75 };
        const float TierMixBonusMultiplier = 1.25f;

        public int GetScore(PlayerSide side) => _ledgers[side].Total;
        public PlayerScoreLedger GetLedger(PlayerSide side) => _ledgers[side];

        // Pure calculation: returns a scored entry without recording it.
        public CompletedRecipeEntry ScoreRecipe(RecipeCardData recipe, List<Card> usedCards, PlayerSide side)
        {
            int baseIngredient = 0;
            int bonusFlat = 0;
            float ingredientMultiplier = recipe.ScoringMultiplier;
            bool tierMixed = false;

            var ingredientTiers = new HashSet<IngredientTier>();

            foreach (var card in usedCards)
            {
                baseIngredient += card.Data.BasePoints + card.TempScoreModifier;

                if (card.Data is IngredientCardData ing)
                {
                    bonusFlat += ing.BonusFlatPoints;
                    ingredientMultiplier *= ing.ScoreMultiplierContribution;
                    ingredientTiers.Add(ing.Tier);
                }
            }

            // Tier mix bonus: ingredients span more than one tier
            if (ingredientTiers.Count > 1) tierMixed = true;
            float tierMixBonus = tierMixed
                ? (baseIngredient + bonusFlat) * (TierMixBonusMultiplier - 1f)
                : 0f;

            int tierBonus = TierBonuses[(int)recipe.Tier];

            var breakdown = new ScoreBreakdown(baseIngredient, bonusFlat, ingredientMultiplier,
                                               tierBonus, tierMixBonus);
            return new CompletedRecipeEntry(recipe, usedCards, breakdown, tierMixed, side);
        }

        public void AddScore(PlayerSide side, CompletedRecipeEntry entry)
        {
            _ledgers[side].Add(entry);
            GameEventBus.Publish(new ScoreChangedEvent
            {
                Side = side,
                NewTotal = _ledgers[side].Total,
                Delta = entry.Breakdown.Total
            });
        }

        public void Reset()
        {
            foreach (var ledger in _ledgers.Values)
            {
                ledger.History.Clear();
            }
            // Recreate ledgers to reset totals
            _ledgers[PlayerSide.Local] = new PlayerScoreLedger();
            _ledgers[PlayerSide.Opponent] = new PlayerScoreLedger();
        }
    }
}
