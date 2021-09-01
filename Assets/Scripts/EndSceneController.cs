using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class EndSceneController : MonoBehaviour
{
    public void SoundExplosionPlay()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_missileExplosion, false);
    }
}
