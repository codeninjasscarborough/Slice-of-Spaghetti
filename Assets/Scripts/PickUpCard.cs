using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpCard : MonoBehaviour
{
    public GameObject hand;
    private RectTransform rectyTransform;

    // This is the Start function
    void Start()
    {
        rectyTransform = GetComponent<RectTransform>();
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

        Animator anims = GetComponent<Animator>();
        if (anims != null)
        {
            anims.enabled = false;
        }

        rectyTransform.anchorMin = new Vector2(0.5f, 0f);
        rectyTransform.anchorMax = new Vector2(0.5f, 0f);

        rectyTransform.pivot = new Vector2(0.5f, 0f);

        rectyTransform.anchoredPosition = Vector2.zero;

        DeckController.Instance.AcceptOfferedCard(hand.GetComponent<HandController>().hand);
    }

    void OnRandomCardAccepted(RandomCardAcceptedEvent e)
    {
        // HI DIS IS BEEG TOMATE HEHE
    }
}


