using System.Collections.Generic;

namespace CardGame
{
    public class RecipeBoard
    {
        readonly List<RecipeSlot> _slots;
        public RecipeCardData Recipe { get; }
        public IReadOnlyList<RecipeSlot> Slots => _slots;
        public bool IsComplete => RecipeValidator.Validate(this).IsValid;

        public RecipeBoard(RecipeCardData recipe)
        {
            Recipe = recipe;
            _slots = new List<RecipeSlot>(recipe.Slots.Count);
            foreach (var req in recipe.Slots)
                _slots.Add(new RecipeSlot(req));
        }

        // Place a card into a specific slot. If the card is a completed recipe (RecipeCardData),
        // it must be removed from the player's CompletedPile before placement.
        public bool TryPlace(Card card, RecipeSlot slot, RecipePile completedPile = null)
        {
            if (card == null || slot == null) return false;
            if (!_slots.Contains(slot)) return false;

            if (card.Data is RecipeCardData && completedPile != null)
                completedPile.Remove(card);

            return slot.TryPlace(card);
        }

        // Place into the first compatible open slot.
        public bool TryPlaceAuto(Card card, RecipePile completedPile = null)
        {
            var slot = RecipeValidator.FindCompatibleSlot(card, this);
            if (slot == null) return false;
            return TryPlace(card, slot, completedPile);
        }

        // Returns false if validation fails. On success: scores the recipe, fires events, clears
        // the board. Utility cards are NOT consumed â€” they are returned via spentUtility with one
        // charge spent. Caller is responsible for moving them back to hand (or discarding if broken).
        public bool TryCompleteRecipe(ScoreManager scoreManager, PlayerSide side, int boardIndex,
                                      out CompletedRecipeEntry entry, out List<Card> spentUtility)
        {
            entry = null;
            spentUtility = null;
            var result = RecipeValidator.Validate(this);
            if (!result.IsValid) return false;

            var usedCards = new List<Card>();
            spentUtility = new List<Card>();

            foreach (var slot in _slots)
            {
                if (slot.PlacedCard == null) continue;
                if (slot.PlacedCard.Data is UtilityCardData)
                    spentUtility.Add(slot.PlacedCard);
                else
                    usedCards.Add(slot.PlacedCard);
            }

            foreach (var util in spentUtility)
                util.UseCharge();

            entry = scoreManager.ScoreRecipe(Recipe, usedCards, side);
            scoreManager.AddScore(side, entry);

            ClearSlots();

            GameEventBus.Publish(new RecipeCompletedEvent { Entry = entry, Side = side });
            return true;
        }

        public void ClearSlots()
        {
            foreach (var slot in _slots)
                slot.Clear();
        }

        // Remove a card from whatever slot it is in (e.g., player picks it back up)
        public bool TryRemoveCard(Card card)
        {
            foreach (var slot in _slots)
            {
                if (slot.PlacedCard == card)
                {
                    slot.Clear();
                    return true;
                }
            }
            return false;
        }
    }
}
