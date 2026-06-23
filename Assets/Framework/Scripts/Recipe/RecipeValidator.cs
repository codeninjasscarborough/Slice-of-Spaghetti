using System.Collections.Generic;

namespace CardGame
{
    public readonly struct ValidationResult
    {
        public readonly bool IsValid;
        public readonly string FailReason;

        public ValidationResult(bool isValid, string failReason = null)
        {
            IsValid = isValid;
            FailReason = failReason;
        }

        public static ValidationResult Success() => new(true);
        public static ValidationResult Fail(string reason) => new(false, reason);
    }

    public static class RecipeValidator
    {
        public static ValidationResult Validate(RecipeBoard board)
        {
            foreach (var slot in board.Slots)
            {
                if (!slot.IsSatisfied)
                    return ValidationResult.Fail($"Slot unsatisfied: {slot.Requirement.MatchMode}");
            }
            return ValidationResult.Success();
        }

        // Returns the first open slot on the board that the card can fill; null if none.
        public static RecipeSlot FindCompatibleSlot(Card card, RecipeBoard board)
        {
            foreach (var slot in board.Slots)
            {
                if (!slot.IsOccupied && slot.Requirement.IsSatisfiedBy(card.Data))
                    return slot;
            }
            return null;
        }

        // Greedily places each card into the first compatible open slot.
        // Returns cards that could not be placed.
        public static List<Card> AutoPlace(IEnumerable<Card> cards, RecipeBoard board)
        {
            var unplaced = new List<Card>();
            foreach (var card in cards)
            {
                var slot = FindCompatibleSlot(card, board);
                if (slot != null)
                    slot.TryPlace(card);
                else
                    unplaced.Add(card);
            }
            return unplaced;
        }
    }
}
