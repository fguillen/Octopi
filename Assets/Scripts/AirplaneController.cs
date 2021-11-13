using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;
using DG.Tweening;

public class AirplaneController : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesFire;
    [SerializeField] ParticleSystem particlesSmoke;

    [SerializeField] Color burntColor = new Color(0.27f, 0.076f, 0.15f);
    [SerializeField] SpriteRenderer colorizable;

    public float velocity = -1;
    bool grabbed = false;
    bool onAir = true;
    bool exploded = false;

    void Start()
    {
        LookTowardsVelocity();
    }

    void Update()
    {
        if(!grabbed)
            transform.Translate(new Vector3(velocity, 0f, 0f) * Time.deltaTime);
    }

    void LookTowardsVelocity()
    {
        if(
            (velocity > 0 && transform.localScale.x > 0) ||
            (velocity < 0 && transform.localScale.x < 0)
        )
        {
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    #region Grabbable
    public void StartGrab()
    {
        Debug.Log("AirplaneController.StartGrab()");
        if(!grabbed)
        {
            // animator.SetBool("Grabbed", true);
            grabbed = true;

            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_grabObject, false);
        }
    }

    public void StopGrab()
    {
        Debug.Log("AirplaneController.StopGrab()");
        if(onAir)
        {
            // animator.SetBool("Grabbed", false);
            grabbed = false;
        }
    }

    public void Thrown()
    {

        Debug.Log("AirplaneController.Thrown()");
        // animator.SetBool("Grabbed", false);
        grabbed = true;
        onAir = false;
        exploded = false;

        SoundThrowPlay();
    }
    #endregion

    #region Crash
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(!exploded && collisionInfo.gameObject.CompareTag("CarGround"))
        {
            StartCoroutine(ExplodeCoroutine());
            exploded = true;
        }
    }

    IEnumerator ExplodeCoroutine()
    {
        // Debug.Log("Car.Explode()");
        particlesExplosion.Play();
        particlesFire.Play();
        particlesSmoke.Play();
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_fire, false);
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_airplaneCrash, false);

        yield return colorizable.DOColor(burntColor, Utils.AddNoise(10.0f)).WaitForCompletion();

        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.SFX_fire, true);
        // animator.SetBool("Moving", false);
    }

    void SoundThrowPlay()
    {
        // TODO Sound: here will be nice to have a airplane falling down sound
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
    #endregion

}
