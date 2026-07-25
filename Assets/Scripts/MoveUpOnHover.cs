using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveUpOnHover : MonoBehaviour
{

    private Vector3 startingSize;
    public float scaleAmount;
    public float scaleDuration = 0.5f;

    private Coroutine activeCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        startingSize = transform.localScale;
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(ScaleElement(startingSize * scaleAmount, scaleDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(ScaleElement(startingSize, scaleDuration));

    }

    private IEnumerator ScaleElement(Vector3 newScale, float duration)
    {
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            transform.localScale = LerpVector3(transform.localScale, newScale, t / duration);
            yield return null;
        }
        transform.localScale = newScale;
        activeCoroutine = null;
    }

    private Vector3 LerpVector3(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }
}
