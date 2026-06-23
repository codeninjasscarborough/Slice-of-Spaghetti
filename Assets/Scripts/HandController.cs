using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CardGame;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HandController : MonoBehaviour  
{
    public Hand hand;

    //Vector2 targetPos = 5;

    //StartCoroutine(AnimatedCardTo(targetPos));

    //private IEnumerator AnimatedCardTo(targetPos)
    //{
    //    Vector2 startPos = card.position;

    //    for (float i = 0; i < animationDuration = 5; i += Time.unscaledDeltaTime)
    //    {

    //        Card.position = Mathf.Lerp(startPos, targetPos, i / animationDuration);
    //        yield return null; S
    //    }
    //    Card.position = targetPos;
    //}

    // Start is called before the first frame update

  

    public void OnClick()
    {
        DeckController.Instance.AcceptOfferedCard(hand);
        Debug.Log("You accepted the card");
    }
    // Update is called once per frame
    
}
