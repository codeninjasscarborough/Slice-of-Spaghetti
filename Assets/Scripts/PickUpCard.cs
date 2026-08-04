using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpCard : MonoBehaviour
{
    [SerializeField]
    private HandController hand;
    private RectTransform rectyTransform;
    private Vector2 startPos;

    [SerializeField]    
    private GameObject cardBase;

    // This is the Start function
    void Start()
    {
        rectyTransform = GetComponent<RectTransform>();
        startPos = rectyTransform.position;
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<RandomCardAcceptedEvent>(OnRandomCardAccepted);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<RandomCardAcceptedEvent>(OnRandomCardAccepted);

    }

    // This is the CardClick function
    public void CardClick()
    {
        if (!DeckController.Instance.HasPendingOffer) return;

        GameObject cardy = Instantiate(cardBase);
        cardy.GetComponent<RectTransform>().SetParent(hand.transform);
        cardy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + (hand.hand.Count * 50f), 120f);
        cardy.GetComponent<Image>().sprite = DeckController.Instance.PendingOffer.Data.Art;

        hand.OnClick();

        Animator anim = GetComponent<Animator>();
        anim.SetBool("Card clicked", false);

        rectyTransform.position = startPos;

    }

    void OnRandomCardAccepted(RandomCardAcceptedEvent e)
    {

    }
}


