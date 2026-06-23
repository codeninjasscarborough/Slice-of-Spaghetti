using UnityEngine;

namespace CardGame.UI
{
    public class CardView : MonoBehaviour
    {
        public Card Card { get; private set; }

        public void Bind(Card card)
        {
            Card = card;
        }
    }
}
