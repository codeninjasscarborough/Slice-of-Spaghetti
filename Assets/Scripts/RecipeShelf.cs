using CardGame;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class RecipeShelf : MonoBehaviour
{
    public Transform contents;
    public TextMeshProUGUI rowPrefab;

    void OnEnable()
    {
        GameEventBus.Subscribe<RecipeCompletedEvent>(MealFinished);
    }

     void OnDisable()
     {
        GameEventBus.Unsubscribe<RecipeCompletedEvent>(MealFinished);
     }

    void MealFinished (RecipeCompletedEvent e)
    {
        if (e.Side != PlayerSide.Local) return;

        TextMeshProUGUI row = Instantiate(rowPrefab, contents);

        string mealName = e.Entry.Recipe.DisplayName;
        int points = e.Entry.Breakdown.Total;

        row.text = mealName + " +" + points;
    }

}
