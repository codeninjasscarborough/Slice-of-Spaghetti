using UnityEngine;
using CardGame;
using System;
using UnityEngine.UI;
using TMPro;

namespace CardGame.UI
{
    

    public class CardView : MonoBehaviour
    {
        public Image arts;
        public TMP_Text title;
        public TMP_Text titleBottom;

        public Action<CardView> onClicked;
        public Card Card { get; private set; }

        void Start()
        {
            Button myButton = GetComponent<Button>();

            if (myButton != null)
            {
                myButton.onClick.AddListener(Clicked);
            }

            title = GetComponentInChildren<TMP_Text>();

            if (title != null)
            {
                title.text = Card.Data.DisplayName;
            }
        }

        public void Show(Card newCard)
        {
            Card = newCard;

            if (arts != null && Card != null)
            {
                arts.sprite = Card.Data.Art;
            }
        }

        public void Bind(Card card)
        {
            Card = card;
        }

        public void Clicked()
        {
            if (Card == null) return;
            if (onClicked != null) onClicked(this);
        }

        public void SetCard(Card c)
        {
            Card = c;
            arts.sprite = Card.Data.Art;

            title.gameObject.SetActive(false);
            titleBottom.gameObject.SetActive(false);

            if (c.Data.TitleIndex == 0)
            {
            title.gameObject.SetActive(true);
                title.text = Card.Data.DisplayName;
            }
            else
            {
                titleBottom.gameObject.SetActive(true);

                titleBottom.text = Card.Data.DisplayName;
            }
        }

    }
}
