using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class MenuController : MonoBehaviour
{
    Animator animator;
    bool visible = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!visible)
                Show();
            else
                Hide();
        }
    }

    public void Show()
    {
        Debug.Log("Menu.Show()");
        visible = true;
        Time.timeScale = 0f;
        animator.SetBool("Visible", true);
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_ui, false);
    }

    public void Hide()
    {
        Debug.Log("Menu.Hide()");
        visible = false;
        Time.timeScale = 1f;
        animator.SetBool("Visible", false);
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_ui, false);
    }
}
