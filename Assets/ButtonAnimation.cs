using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public Animator animator;
    
    public void PlayAnimation()
    {
        animator.SetTrigger("Play");
        Debug.Log("01011001 01101111 01110101 00100111 01110010 01100101 00100000 01100001 01110111 01100101 01110011 01101111 01101101 01100101 00100001 got it?");
    }

}
