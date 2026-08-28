using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardGame;
using TMPro;
using CardGame.UI;


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

    }

    void Cook()
    {

    }
}
