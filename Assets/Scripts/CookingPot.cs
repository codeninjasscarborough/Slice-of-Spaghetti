using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardGame;
using TMPro;
using CardGame.UI;
using Unity.VisualScripting;


public class CookingPot : MonoBehaviour
{
    [Header("Variables")]
    public RecipeCatalog recipeBook;
    public CardView cardPrefab;
    public Button cookStuff;
    public TextMeshProUGUI potLabel;

    public float gapsBetweenCards = 120f;

    List<CardView>inThePot = new List<CardView>();
    RecipeBoard theBoard;

    void Start()
    {
        cookStuff.onClick.AddListener(Cook);
        cookStuff.interactable = false;
        potLabel.text = "Put ingredients in the pot!";
    }

    public void PutInThePot(CardView clicked)
    {
        Card theCard = clicked.Card;
        Hand myHand = GameManager.Instance.LocalPlayer.Hand;

        CardView inPot = Instantiate(cardPrefab, transform);
        if (myHand.Remove(theCard) == false) return;
        inPot.Show(theCard);
        inPot.onClicked = TakeOutOfPot;
        inThePot.Add(inPot);
    }

    public void TakeOutOfPot(CardView clicked)
    {
        Hand myHand = GameManager.Instance.LocalPlayer.Hand;

        if (myHand.TryAdd(clicked.Card) == false)
        {
            Debug.Log("You can't hold anymore cards!");
            return;
        }

        inThePot.Remove(clicked);
        Destroy(clicked.gameObject);

        LayOutPot();
        LookForRecipe();
    }

    void LayOutPot()
    {
        int howMany = inThePot.Count;
        float startX = -(howMany - 1) * gapsBetweenCards / 2f;

        for (int i = 0; i < howMany; i++)
        {
            RectTransform box = inThePot[i].GetComponent<RectTransform>();

            box.anchoredPosition = new Vector2(startX + (i * gapsBetweenCards), 0);
            box.localRotation = Quaternion.identity;
        }
    }

    void LookForRecipe()
    {
        PlayerState me  = GameManager.Instance.LocalPlayer;

        if (theBoard != null)
        {
            me.CloseRecipeBoard(theBoard);
            theBoard = null;
        }

        if(inThePot.Count == 0)
        {
            potLabel.text = ("Please put something in the pot!");
            cookStuff.interactable = false;
            return;
        }

        List<Card> potCards = new List<Card>();
        foreach (CardView v in inThePot)
        {
            potCards.Add(v.Card);
        }

        foreach (RecipeCatalog.RecipeCatalogEntry entry in recipeBook.Entries)
        {
           if (entry.recipe == null) continue;
           if (entry.visible == false) continue;

           if (entry.recipe.Slots.Count != potCards.Count) continue;

           RecipeBoard tryBoard = me.OpenRecipeBoard(entry.recipe);
           List<Card>leftOver = RecipeValidator.AutoPlace(potCards, tryBoard);

           if (leftOver.Count == 0 && tryBoard.IsComplete)
           { 
              theBoard = tryBoard;
              potLabel.text = "Making: " + entry.recipe.DisplayName;
              cookStuff.interactable = true;
              return;
           }

            me.CloseRecipeBoard(tryBoard);
        }

        potLabel.text = inThePot.Count + "These are nice cards, but no recipe is made like this!";
        cookStuff.interactable = false;
    }

    void Cook()
    {
        if (theBoard == null) return;
        if (theBoard.IsComplete == false) return;

        string mealName = theBoard.Recipe.DisplayName;

        // Holy line of code
        GameManager.Instance.RequestCompleteRecipe(theBoard, PlayerSide.Local);

        foreach (CardView v in inThePot)
        {
            Destroy(v.gameObject);
        }
        inThePot.Clear();
        theBoard = null;

        potLabel.text = "You Cooked:" + mealName + "!";
        cookStuff.interactable = false;
    }
}
