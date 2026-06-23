using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

public class GameBusCode : MonoBehaviour
{
    void OnEnable()
    {
        GameEventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
        GameEventBus.Subscribe<RecipeCompletedEvent>(OnRecipeCompleted);
        GameEventBus.Subscribe<TurnPhaseChangedEvent>(OnPhaseChanged);
        GameEventBus.Subscribe<GameEndedEvent>(OnGameEnded);
    }

    void OnDisable()
    {
        GameEventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);
        GameEventBus.Unsubscribe<RecipeCompletedEvent>(OnRecipeCompleted);
        GameEventBus.Unsubscribe<TurnPhaseChangedEvent>(OnPhaseChanged);
        GameEventBus.Unsubscribe<GameEndedEvent>(OnGameEnded);
    }

    void OnScoreChanged(ScoreChangedEvent e)
    {
        Debug.Log("New Score For" + e.Side + ": " + e.NewTotal);
        // Update Score UI 
    }

    void OnRecipeCompleted(RecipeCompletedEvent e)
    {
        Debug.Log(e.Side + "Finished: " + e.Entry.Recipe.DisplayName);
        // Play the animation here 
    }

    void OnPhaseChanged(TurnPhaseChangedEvent e)
    {
        Debug.Log("Phase Changed To:" + e.Current);
        //Show/hide buttons and/or screens depending on phase
    }

    void OnGameEnded(GameEndedEvent e)
    {
        Debug.Log("Winner: " + e.Winner + " with " + e.WinnerScore + " points!");
        //Show the game over screen/animation
    }

}
