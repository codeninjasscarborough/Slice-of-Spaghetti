using UnityEngine;

public class IfClicked : MonoBehaviour
{
    public Animator animator;

    public void OnClick()
    {
        animator.SetTrigger("IfClicked");
    }
}