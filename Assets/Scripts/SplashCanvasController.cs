using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashCanvasController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowControls()
    {
        animator.SetBool("ControlsVisible", true);
    }

    public void HideControls()
    {
        animator.SetBool("ControlsVisible", false);
    }
}
