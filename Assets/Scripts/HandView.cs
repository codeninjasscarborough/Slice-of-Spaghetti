using System.Collections;
using System.Collections.Generic;
using CardGame;
using CardGame.UI;
using UnityEngine;

public class HandView : MonoBehaviour
{
    public CardView cardPrefab;

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
            //newCard.onClicked = 
    }
}
