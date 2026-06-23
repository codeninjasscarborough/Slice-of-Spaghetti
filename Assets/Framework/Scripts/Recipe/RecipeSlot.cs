namespace CardGame
{
    public class RecipeSlot
    {
        public RecipeRequirement Requirement { get; }
        public Card PlacedCard { get; private set; }

        public bool IsOccupied => PlacedCard != null;
        public bool IsSatisfied => IsOccupied && Requirement.IsSatisfiedBy(PlacedCard.Data);

        public RecipeSlot(RecipeRequirement requirement)
        {
            Requirement = requirement;
        }

        public bool TryPlace(Card card)
        {
            if (IsOccupied) return false;
            if (!Requirement.IsSatisfiedBy(card.Data)) return false;

            PlacedCard = card;
            card.Zone = CardZone.RecipeBoard;
            return true;
        }

        // Debug only â€” bypasses requirement check
        public void ForcePlace(Card card)
        {
            PlacedCard = card;
            if (card != null) card.Zone = CardZone.RecipeBoard;
        }

        public Card Clear()
        {
            var card = PlacedCard;
            PlacedCard = null;
            return card;
        }
    }
}
