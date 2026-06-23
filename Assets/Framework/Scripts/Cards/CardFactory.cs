using System.Collections.Generic;

namespace CardGame
{
    public static class CardFactory
    {
        public static Card CreateCard(CardData data, CardZone zone = CardZone.Deck) =>
            new Card(data, zone);

        public static List<Card> CreateCards(CardData data, int count, CardZone zone = CardZone.Deck)
        {
            var list = new List<Card>(count);
            for (int i = 0; i < count; i++)
                list.Add(new Card(data, zone));
            return list;
        }

        public static Deck BuildDeckFromDatabase(CardDatabase database)
        {
            var deck = new Deck();
            var allCards = new List<Card>();

            foreach (var entry in database.Entries)
            {
                if (entry.data == null) continue;
                allCards.AddRange(CreateCards(entry.data, entry.copies));
            }

            deck.AddRange(allCards);
            deck.Shuffle();
            return deck;
        }

        // Creates a "completed recipe" card that can be stored in the RecipePile
        // and later consumed as a sub-ingredient in a higher-tier recipe.
        public static Card CreateCompletedRecipeCard(RecipeCardData recipe, List<Card> usedCards)
        {
            var card = new Card(recipe, CardZone.RecipePile);
            return card;
        }
    }
}
