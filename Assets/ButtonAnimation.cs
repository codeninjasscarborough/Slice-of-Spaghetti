using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public Animator animator;
    
    public void PlayAnimation()
    {
        animator.SetTrigger("Play");
        Debug.Log("Hi guys omg i love potatoes and like i am nine years old and stuff. Did you know that snakes are cute and I don't care if you say otherwise? Candy can be good but it isn't that good for you.");
    }

}
