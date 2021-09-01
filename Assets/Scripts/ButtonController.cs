using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityCore.Audio;

public class ButtonController : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(SoundOnClick);
    }

    void SoundOnClick()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_ui, false);
    }
}
