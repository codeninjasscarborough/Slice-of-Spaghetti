using System.Collections.Generic;

namespace CardGame
{
    public class Hand
    {
        readonly List<Card> _cards = new();
        readonly PlayerSide _side;

        int _maxSize = 27;

        public int MaxSize
        {
            get => _maxSize;
            set => _maxSize = value;
        }

        public int Count => _cards.Count;
        public bool IsFull => _cards.Count >= _maxSize;
        public IReadOnlyList<Card> Cards => _cards;

        public Hand(PlayerSide side, int maxSize = 27)
        {
            _side = side;
            _maxSize = maxSize;
        }

        public bool TryAdd(Card card)
        {
            if (IsFull)
            {
                GameEventBus.Publish(new HandFullEvent { Side = _side });
                return false;
            }
            card.Zone = CardZone.Hand;
            _cards.Add(card);
            PublishChanged();
            return true;
        }

        // Draws cards from deck until hand is full or deck is empty; returns how many were drawn
        public int DrawUpTo(Deck deck, int count = int.MaxValue)
        {
            int drawn = 0;
            while (drawn < count && !IsFull && !deck.IsEmpty)
            {
                var card = deck.Draw();
                if (card == null) break;
                card.Zone = CardZone.Hand;
                _cards.Add(card);
                drawn++;
            }
            if (drawn > 0) PublishChanged();
            return drawn;
        }

        public bool Remove(Card card)
        {
            if (!_cards.Remove(card)) return false;
            PublishChanged();
            return true;
        }

        public List<Card> DiscardAll(DiscardPile pile)
        {
            var removed = new List<Card>(_cards);
            foreach (var c in removed)
                pile.Add(c);
            _cards.Clear();
            PublishChanged();
            return removed;
        }

        public bool Contains(Card card) => _cards.Contains(card);

        void PublishChanged() =>
            GameEventBus.Publish(new HandChangedEvent { Side = _side, Hand = new List<Card>(_cards) });
    }
}
