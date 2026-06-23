using System;
using UnityEngine;

namespace CardGame
{
    // Attached to the clickable deck object in the scene.
    // Manages the three-step offer flow: click â†’ card offered â†’ player takes or returns.
    public class DeckController : Singleton<DeckController>
    {
        Deck _deck;
        Card _offeredCard;
        PlayerSide _currentSide;
        bool _offerPending;

        public event Action OnAskedForCard;

        protected override void Awake()
        {
            base.Awake();

        }

        public void Initialize(Deck deck) => _deck = deck;

        // Called by GameManager when it's the active player's draw phase.
        public void SetActiveSide(PlayerSide side) => _currentSide = side;

        // UI calls this when the player clicks the deck.
        public void OnDeckClicked()
        {
            if (_deck == null || _deck.IsEmpty || _offerPending) return;

            _offeredCard = _deck.Draw();
            if (_offeredCard == null) return;

            _offeredCard.Zone = CardZone.Offered;
            _offerPending = true;

            GameEventBus.Publish(new RandomCardOfferedEvent
            {
                OfferedCard = _offeredCard,
                Side = _currentSide
            });

            OnAskedForCard?.Invoke();

        }

        // Player accepts the offered card; it moves to their hand.
        public bool AcceptOfferedCard(Hand targetHand)
        {
            if (!_offerPending || _offeredCard == null) return false;

            bool added = targetHand.TryAdd(_offeredCard);
            if (added)
            {
                GameEventBus.Publish(new RandomCardAcceptedEvent
                {
                    AcceptedCard = _offeredCard,
                    Side = _currentSide
                });
            }
            else
            {
                // Hand full â€” return the card to the top of the deck
                _deck.AddToTop(_offeredCard);
            }

            _offeredCard = null;
            _offerPending = false;
            return added;
        }

        // Player declines; the card is shuffled back into the deck at a random position.
        // Returns false (and does nothing) if the offered card is a Recipe â€” recipes must be taken.
        public bool DeclineOfferedCard()
        {
            if (!_offerPending || _offeredCard == null) return false;

            if (_offeredCard.Data.CardType == CardType.Recipe) return false;

            int insertIndex = UnityEngine.Random.Range(0, _deck.Count + 1);
            _deck.InsertAt(insertIndex, _offeredCard);

            GameEventBus.Publish(new RandomCardDeclinedEvent
            {
                DeclinedCard = _offeredCard,
                Side = _currentSide
            });

            _offeredCard = null;
            _offerPending = false;
            return true;
        }

        public bool HasPendingOffer => _offerPending;
        public Card PendingOffer => _offeredCard;
    }
}
