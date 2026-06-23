using System.Collections.Generic;

namespace CardGame
{
    // Pile of completed recipe cards; each can be consumed as a sub-ingredient.
    public class RecipePile
    {
        readonly List<Card> _cards = new();

        public int Count => _cards.Count;
        public IReadOnlyList<Card> Cards => _cards;

        public void Add(Card card)
        {
            card.Zone = CardZone.RecipePile;
            _cards.Add(card);
        }

        public bool Remove(Card card) => _cards.Remove(card);

        public bool Contains(Card card) => _cards.Contains(card);

        public void Clear() => _cards.Clear();
    }

    public class PlayerState
    {
        public PlayerSide Side { get; }
        public Hand Hand { get; }

        // CompletedPile holds recipe cards that have been finished and can be
        // consumed as sub-ingredients in higher-tier recipes.
        public RecipePile CompletedPile { get; } = new();

        public List<RecipeBoard> RecipeBoards { get; } = new();

        // Per-turn and per-game stats
        public int TurnCount { get; private set; }
        public int RecipesCompleted { get; private set; }
        public int CardsPlayed { get; private set; }

        public PlayerState(PlayerSide side, int maxHandSize = 27)
        {
            Side = side;
            Hand = new Hand(side, maxHandSize);
        }

        public RecipeBoard OpenRecipeBoard(RecipeCardData recipe)
        {
            var board = new RecipeBoard(recipe);
            RecipeBoards.Add(board);
            GameEventBus.Publish(new RecipeBoardOpenedEvent
            {
                Side = Side,
                BoardIndex = RecipeBoards.Count - 1,
                Recipe = recipe
            });
            return board;
        }

        public bool CloseRecipeBoard(RecipeBoard board)
        {
            board.ClearSlots();
            return RecipeBoards.Remove(board);
        }

        // Called when a recipe is successfully completed.
        public void OnRecipeCompleted(CompletedRecipeEntry entry, RecipeBoard board)
        {
            RecipesCompleted++;
            var completedCard = CardFactory.CreateCompletedRecipeCard(entry.Recipe, new List<Card>(entry.CardsUsed));
            CompletedPile.Add(completedCard);
            RecipeBoards.Remove(board);
        }

        public void IncrementTurn() => TurnCount++;
        public void RecordCardPlayed() => CardsPlayed++;

        public void ResetTurnStats() => CardsPlayed = 0;
    }
}
