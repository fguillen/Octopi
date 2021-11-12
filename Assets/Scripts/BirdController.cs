using UnityEngine;
using UnityCore.Audio;

public class BirdController : MonoBehaviour
{
    public float velocity = -1;
    Animator animator;
    bool grabbed = false;
    bool onAir = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.SetBool("Flying", true);
        animator.SetBool("Grabbed", false);
        FlipTowardsVelocity();
    }

    void Update()
    {
        if(!grabbed)
            transform.Translate(new Vector3(velocity, 0f, 0f) * Time.deltaTime);
    }

    void FlipTowardsVelocity()
    {
        if(
            (velocity > 0 && transform.localScale.x > 0) ||
            (velocity < 0 && transform.localScale.x < 0)
        )
        {
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void StartGrab()
    {
        if(!grabbed)
        {
            animator.SetBool("Grabbed", true);
            grabbed = true;

            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_birdGrab, false);
        }
    }

    public void StopGrab()
    {
        Debug.Log("BirdController.StopGrab()");
        if(onAir)
        {
            animator.SetBool("Grabbed", false);
            grabbed = false;
        }
    }

    public void Thrown()
    {
        if(onAir)
        {
            Debug.Log("BirdController.Thrown()");
            animator.SetBool("Grabbed", false);
            grabbed = true;
            onAir = false;

            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_birdCrash, false);
        }
    }
}
