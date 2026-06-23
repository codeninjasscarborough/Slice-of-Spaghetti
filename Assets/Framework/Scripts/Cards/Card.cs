namespace CardGame
{
    public class Card
    {
        public CardData Data { get; }
        public CardZone Zone { get; set; }
        public bool Exhausted { get; set; }
        public int TempScoreModifier { get; set; }
        public int CurrentDurability { get; private set; }

        public Card(CardData data, CardZone initialZone = CardZone.Deck)
        {
            Data = data;
            Zone = initialZone;
            if (data is UtilityCardData util)
                CurrentDurability = util.MaxDurability;
        }

        // Spends one durability charge. Returns true if the card is still usable.
        public bool UseCharge()
        {
            if (CurrentDurability > 0) CurrentDurability--;
            return CurrentDurability > 0;
        }

        public void ResetTemporaryState()
        {
            Exhausted = false;
            TempScoreModifier = 0;
        }

        public override string ToString()
        {
            string tier = Data switch
            {
                IngredientCardData ing => $"{ing.Tier} ",
                RecipeCardData rec    => $"{rec.Tier} ",
                _                     => ""
            };
            return $"[{tier}{Data?.CardType}] {Data?.DisplayName}";
        }
    }
}
