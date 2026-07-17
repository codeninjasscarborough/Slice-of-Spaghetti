using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardToBottom : MonoBehaviour
{
    public RectTransform targetCard;

    public void MoveElementToBottom()
    {
        if (targetCard == null) return;

        targetCard.anchorMin = new Vector2(0.5f, 0f);
        targetCard.anchorMax = new Vector2(0.5f, 0f);

        targetCard.pivot = new Vector2(0.5f, 0f);
        targetCard.anchoredPosition = Vector2.zero;

    }
}