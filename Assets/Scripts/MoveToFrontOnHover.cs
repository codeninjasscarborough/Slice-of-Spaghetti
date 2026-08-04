using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MoveToFrontOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Vector3 originalScale;
    private HorizontalLayoutGroup handy;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;

        handy = GetComponentInParent<HorizontalLayoutGroup>();

        // mrow :3
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
       handy.enabled = false;
        // up
        StartCoroutine(ScaleElement(1.2f, 1f));

        Debug.Log("It Works hehe");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        handy.enabled = true;
        // down
        StartCoroutine(ScaleElement(1f, 1f));

        Debug.Log("it works baby");
    }

    private IEnumerator ScaleElement(float amt, float duration)
    {
        
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            transform.localScale = LerpVector3(originalScale, originalScale * amt, t / duration);
            yield return null;
        }
        transform.localPosition = originalScale * amt;
        yield return null;
        Debug.Log("It Works lalala");
    }

    private Vector3 LerpVector3(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }
}
