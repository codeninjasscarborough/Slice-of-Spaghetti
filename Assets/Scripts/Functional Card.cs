using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.UI;

public class FunctionalCard : MonoBehaviour
{
    [Header("Database")]
    public CardDatabase cardDatabase;
    public RecipeCatalog recipeCatalog;
    private Deck deck = new();

    [Header("Card")]
    public int numOfCardsInDeck;
    public Image card;

    [SerializeField] private Animator cardAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Card c;

        foreach (var entry in cardDatabase.Entries)
        {
            c = new Card(entry.data);

            for (int i = 0; i < entry.copies; i++) {
                deck.AddToTop(c);
            }
        }

        deck.Shuffle();
    }

    private void OnEnable()
    {
        DeckController.Instance.Initialize(deck);
        DeckController.Instance.OnAskedForCard += OpenCard;
    }

    private void OnDisable()
    {
       // THAT LITERALLY HAS NO USE SO FAR -> 
        //DeckController.Instance.OnAskedForCard -= OpenCard;
    }

    void OpenCard()
    {
        card.sprite = DeckController.Instance.PendingOffer.Data.Art;
        cardAnimator?.SetBool("Card clicked", true);

    }

}

