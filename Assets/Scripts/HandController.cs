using System.Collections;
using CardGame;
using UnityEngine;

public class HandController : MonoBehaviour  
{
    public Hand hand;
    public float xOffset = -5f;

    // Start is called before the first frame update
    private void Awake()
    {

        hand = new Hand(PlayerSide.Local);

        Debug.Log("Da Beeg Tomato Is Rotten.");
    }

    public void OnClick()
    {
        DeckController.Instance.AcceptOfferedCard(hand);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset * hand.Count, 50f);
    }
    // Update is called once per frame
    
}
