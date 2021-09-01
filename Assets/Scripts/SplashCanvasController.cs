using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

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

    public void SoundBeachPlay()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.BCKGR_beach, true);
    }
}
