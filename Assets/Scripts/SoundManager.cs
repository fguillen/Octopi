using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        BackgroundCity,
        BackgroundBeach,

        PlayerMove,
        PlayerTentacle,
        PlayerHit,
        PlayerJump,
        PlayerFallHit,

        SoldierBulletShot,
        HelicopterMissileShot,
        HelicopterEngine,
        HelicopterExplode,
        TankEngine,
        TankCannonShot,
        TankExplode,

        WallDestroy,
        WindowDestroy,

        BirdYell,
        BirdCrush,
        CarHorn,
        CarCrush,
        HumanScream,
        HumanCrush,
    }
   public static void PlaySound (Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    } 
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAssets : MonoBehaviour
{
    private static SoundAssets _i;

    public static SoundAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("SoundAssets")) as GameObject).GetComponent<SoundAssets>();
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}*/


