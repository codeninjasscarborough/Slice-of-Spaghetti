using UnityEngine;

namespace CardGame
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Data")]
        [SerializeField] CardDatabase cardDatabase;
        [SerializeField] RecipeCatalog recipeCatalog;

        [Header("Scene References")]
        [SerializeField] DeckController deckController;
        [SerializeField] UI.BoardLayoutZones boardLayoutZones;

        [Header("Settings")]
        [SerializeField] int cardsDrawnPerTurn = 3;
        [SerializeField] int winScoreThreshold = 200;

        public Deck SharedDeck { get; private set; }
        public DiscardPile SharedDiscard { get; private set; }
        public ScoreManager ScoreManager { get; private set; }
        public RecipeGuideManager GuideManager { get; private set; }
        public PlayerState LocalPlayer { get; private set; }
        public PlayerState Opponent { get; private set; }

        TurnStateMachine _turnMachine;

        protected override void Awake()
        {
            base.Awake();
            InitializeSystems();
        }

        void InitializeSystems()
        {
            SharedDeck = CardFactory.BuildDeckFromDatabase(cardDatabase);
            SharedDiscard = new DiscardPile();
            ScoreManager = new ScoreManager();
            LocalPlayer = new PlayerState(PlayerSide.Local);
            Opponent = new PlayerState(PlayerSide.Opponent);
            GuideManager = new RecipeGuideManager(recipeCatalog);

            deckController.Initialize(SharedDeck);
            _turnMachine = new TurnStateMachine(this, cardsDrawnPerTurn, winScoreThreshold);
        }

        void Start() => _turnMachine.Begin();

        void OnDestroy() => GameEventBus.ClearAll();

        // â”€â”€ Public API for UI and external systems â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public PlayerState GetPlayer(PlayerSide side) =>
            side == PlayerSide.Local ? LocalPlayer : Opponent;

        public void RequestEndPlayPhase() => _turnMachine.AdvanceFromPlay();

        public void RequestCompleteRecipe(RecipeBoard board, PlayerSide side)
        {
            var player = GetPlayer(side);
            if (board.TryCompleteRecipe(ScoreManager, side, player.RecipeBoards.IndexOf(board),
                                        out var entry, out var spentUtility))
            {
                player.OnRecipeCompleted(entry, board);

                foreach (var util in spentUtility)
                {
                    if (util.CurrentDurability > 0)
                        player.Hand.TryAdd(util);
                    else
                    {
                        SharedDiscard.Add(util);
                        GameEventBus.Publish(new UtilityCardBrokenEvent { Card = util, Side = side });
                    }
                }

                GuideManager.MarkDiscovered(entry.Recipe);
                foreach (var unlocked in entry.Recipe.UnlocksRecipes)
                    GuideManager.MarkDiscovered(unlocked);

                if (ScoreManager.GetScore(side) >= winScoreThreshold)
                    _turnMachine.TriggerGameEnd(side);
            }
        }

        // â”€â”€ Turn State Machine â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        class TurnStateMachine
        {
            readonly GameManager _gm;
            readonly int _drawCount;
            readonly int _winThreshold;

            GamePhase _phase = GamePhase.Setup;
            PlayerSide _activePlayer = PlayerSide.Local;

            public TurnStateMachine(GameManager gm, int drawCount, int winThreshold)
            {
                _gm = gm;
                _drawCount = drawCount;
                _winThreshold = winThreshold;
            }

            public void Begin()
            {
                GameEventBus.Publish(new GameStartedEvent());
                EnterDraw();
            }

            void EnterDraw()
            {
                SetPhase(GamePhase.Draw);
                var player = _gm.GetPlayer(_activePlayer);
                player.IncrementTurn();

                int drawn = player.Hand.DrawUpTo(_gm.SharedDeck, _drawCount);

                // If deck ran out mid-draw, recycle discard and finish drawing
                if (drawn < _drawCount && _gm.SharedDeck.IsEmpty)
                {
                    _gm.SharedDiscard.RecycleIntoDeck(_gm.SharedDeck);
                    player.Hand.DrawUpTo(_gm.SharedDeck, _drawCount - drawn);
                }

                _gm.deckController.SetActiveSide(_activePlayer);
                EnterPlay();
            }

            public void EnterPlay() => SetPhase(GamePhase.Play);

            // Called by UI when the active player ends their play phase.
            public void AdvanceFromPlay()
            {
                if (_phase != GamePhase.Play) return;
                EnterScore();
            }

            void EnterScore()
            {
                SetPhase(GamePhase.Score);
                // Score phase auto-advances â€” all active boards have already been completed
                // via RequestCompleteRecipe() calls during Play. Just move to End.
                _gm.GetPlayer(_activePlayer).ResetTurnStats();
                EnterEnd();
            }

            void EnterEnd()
            {
                SetPhase(GamePhase.End);

                // Check win condition
                if (_gm.ScoreManager.GetScore(_activePlayer) >= _winThreshold)
                {
                    TriggerGameEnd(_activePlayer);
                    return;
                }

                // Switch active player and start next turn
                _activePlayer = _activePlayer == PlayerSide.Local
                    ? PlayerSide.Opponent
                    : PlayerSide.Local;

                EnterDraw();
            }

            public void TriggerGameEnd(PlayerSide winner)
            {
                SetPhase(GamePhase.End);
                GameEventBus.Publish(new GameEndedEvent
                {
                    Winner = winner,
                    WinnerScore = _gm.ScoreManager.GetScore(winner)
                });
            }

            void SetPhase(GamePhase next)
            {
                var prev = _phase;
                _phase = next;
                GameEventBus.Publish(new TurnPhaseChangedEvent
                {
                    Previous = prev,
                    Current = next,
                    ActivePlayer = _activePlayer
                });
                Debug.Log($"[GameManager] Phase: {prev} <> {next} | Active: {_activePlayer}");
            }
        }
    }
}
