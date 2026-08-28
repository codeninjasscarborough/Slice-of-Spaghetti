using UnityEngine;
using CardGame;
using System;
using UnityEngine.UI;

namespace CardGame.UI
{
    

    public class CardView : MonoBehaviour
    {
        public Image arts;
        public Card card;

        public Action<CardView> onClicked;
        public Card Card { get; private set; }

        void Start()
        {
            Button myButton = GetComponent<Button>();

            if (myButton != null)
            {
                myButton.onClick.AddListener(Clicked);
            }
        }

        public void Show(Card newCard)
        {
            card = newCard;

            if (arts != null && card != null)
            {
                arts.sprite = card.Data.Art;
            }
        }

        public void Bind(Card card)
        {
            Card = card;
        }

        public void Clicked()
        {
            if (card == null) return;
            if (onClicked != null) onClicked(this);
        }


    }
}
