using System.Collections;
using System.Collections.Generic;
using CardGame;
using CardGame.UI;
using UnityEngine;

public class HandView : MonoBehaviour
{
    public CardView cardPrefab;
    public CookingPot pot;

    public float spacing = 90f;
    public float biggestWidth = 1400f;
    public float tilt = 4f;

    List<CardView> myCards = new List<CardView>();

    void OnEnable()
    {
        GameEventBus.Subscribe<HandChangedEvent>(HandChanged);
    }

    void OnDisable()
    {
        GameEventBus.Unsubscribe<HandChangedEvent>(HandChanged);
    }

    void HandChanged(HandChangedEvent e)
    {
        if (e.Side != PlayerSide.Local) return;

        foreach (CardView old in myCards)
        {
            Destroy(old.gameObject);
        }

        myCards.Clear();

        foreach (Card c in e.Hand)
        {
            CardView newCard = Instantiate(cardPrefab, transform);
            newCard.Show(c);
            newCard.onClicked = CardWasClicked;
            myCards.Add(newCard);
        }

        LayOutCards();
    }

    void LayOutCards()
    {
        int howMany = myCards.Count;
        Debug.Log(howMany + (" can you debug.log how many?"));
        if (howMany == 0) return;

       float gap = Mathf.Min(spacing, biggestWidth/howMany);
       float startX = -(howMany - 1) * gap / 2f;
       
        for (int i = 0; i < howMany; i++)
        {
            RectTransform box = myCards[i].GetComponent<RectTransform>();
            float howFar = 0.5f;

             if(howMany > 1) howFar = (float)i / (howMany - 1);

            float x = startX + (i * gap);
            float y = Mathf.Sin(howFar * Mathf.PI) * 25f;

            box.anchoredPosition = new Vector2(x, y);
            box.localRotation = Quaternion.Euler(0,0, Mathf.Lerp(tilt, -tilt, howFar));
            box.SetSiblingIndex(i);
        }
    }

    void CardWasClicked(CardView clicked)
    {
        if (pot != null) pot.PutInThePot(clicked);
    }
}
