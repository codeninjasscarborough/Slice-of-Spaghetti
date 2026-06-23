using System.Collections.Generic;

namespace CardGame
{
    public class DiscardPile
    {
        readonly List<Card> _cards = new();

        public int Count => _cards.Count;
        public IReadOnlyList<Card> Cards => _cards;

        public void Add(Card card)
        {
            card.Zone = CardZone.Discard;
            _cards.Add(card);
        }

        public void RecycleIntoDeck(Deck deck)
        {
            foreach (var c in _cards)
                deck.AddToBottom(c);
            _cards.Clear();
            deck.Shuffle();
        }

        public bool Remove(Card card) => _cards.Remove(card);

        public void Clear() => _cards.Clear();
    }
}
