using System.Collections;
using System.Collections.Generic;
using CardGame;
using CardGame.UI;
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

        //GameObject cardy = Instantiate(cardBase);
          
//|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/|\\
// problem no.1                                                                                                                                                 //1\\
//debug.log aint working man                                                                                                                                    //2 \\
// BODYGUARD TO KEEP IN CHECK                                                                                                                                   //3  \\
// !?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! "                                                                                            //4   \\
// |/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WA//5    \\                                                                                                                 //
//|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/|!                                     //6     \\
                                                                         //|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\/|!                            //7      \\
//cardy.GetComponent<RectTransform>().SetParent(hand.transform);           //|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\/|! (ENGINE)                   //8       || 
                                                                         //|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|\/|!                            //9      //
//|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/|!                                     //1     //
// |/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WALL|\||/|DIAMOND & CARBON WA//2    //                                                                                                                      
//!?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! TRAPS !?! "                                                                                              //3  //
// BODYGUARD TO KEEP IN CHECK                                                                                                                                   //4  //
//debug.log aint working man                                                                                                                                    //5 //
// problem no.1                                                                                                                                                 //6//
//|\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/|\/||\|/|\|/|\|/|\|/||\|/|\|/|\|/|\|/|\/||\|//

        //cardy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + (hand.hand.Count * 50f), 120f);
        //cardy.GetComponent<CardView>().SetCard(DeckController.Instance.PendingOffer); 

        hand.OnClick();

        Animator anim = GetComponent<Animator>();
        anim.SetBool("Card clicked", false);

        rectyTransform.position = startPos;

    } 

    void OnRandomCardAccepted(RandomCardAcceptedEvent e)
    {

    }
}


