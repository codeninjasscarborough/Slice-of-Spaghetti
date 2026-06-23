using System.Collections.Generic;

namespace CardGame
{
    // Deck events
    public struct DeckShuffledEvent { }

    public struct CardDrawnEvent
    {
        public Card DrawnCard;
        public PlayerSide Side;
        public int RemainingInDeck;
    }

    public struct DeckEmptyEvent { }

    // Hand events
    public struct HandChangedEvent
    {
        public PlayerSide Side;
        public List<Card> Hand;
    }

    public struct HandFullEvent
    {
        public PlayerSide Side;
    }

    // Board / recipe events
    public struct CardPlacedOnBoardEvent
    {
        public Card PlacedCard;
        public PlayerSide Side;
        public int BoardIndex;
        public int SlotIndex;
    }

    public struct RecipeCompletedEvent
    {
        public CompletedRecipeEntry Entry;
        public PlayerSide Side;
    }

    public struct RecipeBoardOpenedEvent
    {
        public PlayerSide Side;
        public int BoardIndex;
        public RecipeCardData Recipe;
    }

    // Score events
    public struct ScoreChangedEvent
    {
        public PlayerSide Side;
        public int NewTotal;
        public int Delta;
    }

    // Phase / turn events
    public struct TurnPhaseChangedEvent
    {
        public GamePhase Previous;
        public GamePhase Current;
        public PlayerSide ActivePlayer;
    }

    public struct GameStartedEvent { }

    public struct GameEndedEvent
    {
        public PlayerSide Winner;
        public int WinnerScore;
    }

    // Deck draw-offer events (DeckController three-step flow)
    public struct RandomCardOfferedEvent
    {
        public Card OfferedCard;
        public PlayerSide Side;
    }

    public struct RandomCardAcceptedEvent
    {
        public Card AcceptedCard;
        public PlayerSide Side;
    }

    public struct RandomCardDeclinedEvent
    {
        public Card DeclinedCard;
        public PlayerSide Side;
    }

    // Guide events
    public struct GuideOpenedEvent
    {
        public PlayerSide Side;
    }

    public struct GuideClosedEvent
    {
        public PlayerSide Side;
    }

    public struct RecipeDiscoveredEvent
    {
        public RecipeCardData Recipe;
        public PlayerSide Side;
    }

    // Fired when a utility card's durability reaches zero after use.
    public struct UtilityCardBrokenEvent
    {
        public Card Card;
        public PlayerSide Side;
    }
}
