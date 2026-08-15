 using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using CardGame;

public class Disable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform recty;

    private Canvas canvas;

    private Transform originalParent;


    private void Awake()
    {

        recty = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true)
;        
    }

    public void OnDrag(PointerEventData eventData)
    {
        recty.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != canvas.transform) return;
        {
            transform.SetParent(originalParent, true);
        }
    }

    
    // DO NOT TOUCH

    /*public void OnDragStart(PointerEventData data)
    {
        originalPos = recty.anchoredPosition;
        isDragging = true;
    }

    public void OnCardDragged(PointerEventData data)
    {
        if (recty == null || disableArea == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            disableArea,
            data.position,
            data.pressEventCamera,
            out Vector2 localPoint
            );

        if (disableArea.rect.Contains(localPoint))
        {
            if (deleteCard)
            {
                enabled = false;
            }
            
            return;
        }

        // Drag Properties
        recty.anchoredPosition += data.delta;
    } */

    // DO NOT TOUCH


}
