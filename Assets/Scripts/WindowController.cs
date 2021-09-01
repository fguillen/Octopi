using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityCore.Audio;

public class WindowController : MonoBehaviour
{
    [SerializeField] GameObject windowBrokenPrefab;
    [SerializeField] Color burntColor = new Color(0.27f, 0.076f, 0.15f);
    [SerializeField] SpriteRenderer colorizable;
    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesSmoke;

    bool onWall = true;

    public void StartGrab()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_grabObject, false);
    }

    public void StopGrab()
    {
    }

    public void Thrown()
    {
        if(onWall)
        {
            Instantiate(windowBrokenPrefab, transform.position, Quaternion.identity, transform.parent);
            onWall = false;
        }{
            particlesExplosion.Play();
            PlayThrowSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(
            collisionInfo.gameObject.CompareTag("WindowGround")
        )
        {
            StartCoroutine(ExplodeCoroutine());
        }
    }

    IEnumerator ExplodeCoroutine()
    {
        // Debug.Log("Car.Explode()");
        particlesExplosion.Play();
        particlesSmoke.Play();

        PlayCrashSound();

        gameObject.layer = LayerMask.NameToLayer("WindowsHidden");

        yield return colorizable.DOColor(burntColor, 0.5f).WaitForCompletion();
    }

    void PlayThrowSound()
    {
        int windowPullClip = Random.Range(1, 9);

        switch (windowPullClip)
        {
            case 1:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_01, false);
                break;
            case 2:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_02, false);
                break;
            case 3:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_03, false);
                break;
            case 4:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_04, false);
                break;
            case 5:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_05, false);
                break;
            case 6:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_06, false);
                break;
            case 7:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_07, false);
                break;
            case 8:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowPull_08, false);
                break;
        }
    }

    void PlayCrashSound()
    {
        int windowCrashClip = Random.Range(1, 4);
        switch (windowCrashClip)
        {
            case 1:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowCrash_01, false);
                break;
            case 2:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowCrash_02, false);
                break;
            case 3:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_windowCrash_03, false);
                break;
        }
    }
}
