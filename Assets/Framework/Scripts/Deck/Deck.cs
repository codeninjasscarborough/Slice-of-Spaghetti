using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Deck
    {
        readonly List<Card> _cards = new();

        public int Count => _cards.Count;
        public bool IsEmpty => _cards.Count == 0;

        // Index 0 is the top of the deck
        public Card PeekTop() => _cards.Count > 0 ? _cards[0] : null;

        public void AddToTop(Card card)
        {
            card.Zone = CardZone.Deck;
            _cards.Insert(0, card);
        }

        public void AddToBottom(Card card)
        {
            card.Zone = CardZone.Deck;
            _cards.Add(card);
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            foreach (var c in cards)
            {
                c.Zone = CardZone.Deck;
                _cards.Add(c);
            }
        }

        public Card Draw()
        {
            if (_cards.Count == 0)
            {
                GameEventBus.Publish(new DeckEmptyEvent());
                return null;
            }

            var card = _cards[0];
            _cards.RemoveAt(0);
            GameEventBus.Publish(new CardDrawnEvent
            {
                DrawnCard = card,
                RemainingInDeck = _cards.Count
            });
            return card;
        }

        public Card DrawAt(int index)
        {
            if (index < 0 || index >= _cards.Count) return null;
            var card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }

        public void Remove(Card card) => _cards.Remove(card);

        public void InsertAt(int index, Card card)
        {
            index = Mathf.Clamp(index, 0, _cards.Count);
            card.Zone = CardZone.Deck;
            _cards.Insert(index, card);
        }

        public void Shuffle()
        {
            var rng = new System.Random();
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
            GameEventBus.Publish(new DeckShuffledEvent());
        }

        public IReadOnlyList<Card> Cards => _cards;
    }
}
