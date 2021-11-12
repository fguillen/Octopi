using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class AirplaneController : MonoBehaviour
{
    public float velocity = -1;
    bool grabbed = false;
    bool onAir = true;

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

            // TODO Sound: put airplaneGrab sound
            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_birdGrab, false);
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
        if(onAir)
        {
            Debug.Log("AirplaneController.Thrown()");
            // animator.SetBool("Grabbed", false);
            grabbed = true;
            onAir = false;

            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_airplaneCrash, false);
        }
    }
    #endregion

}
