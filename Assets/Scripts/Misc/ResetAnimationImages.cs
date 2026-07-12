using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetAnimationImages : MonoBehaviour
{
    private Sprite startImage;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        startImage = image.sprite;

    }

    public void SwitchBack()
    {
        GetComponent<Animator>().SetTrigger("Loaded");
        image.sprite = startImage;
        Debug.Log("Hi Guys This is da BEEG tomato :)");
    }
}