using UnityEngine;
using UnityCore.Audio;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D TheRigidbody;

    void Awake()
    {
        this.TheRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotateTowardsDirection();
    }

    void RotateTowardsDirection()
    {
        Vector2 moveDirection = TheRigidbody.velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.gameObject.GetComponent<PlayerController>().HitByBullet(collisionInfo.gameObject.transform.position, TheRigidbody.velocity.normalized);
        }

        Explode();
        SoundImpactPlay();
    }

    void Explode()
    {
        Destroy(gameObject);
    }

    void SoundImpactPlay()
    {
        int bulletImpactClip = Random.Range(1, 6);
        switch (bulletImpactClip)
        {
            case 1:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_bulletImpact_01, false);
                break;
            case 2:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_bulletImpact_02, false);
                break;
            case 3:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_bulletImpact_03, false);
                break;
            case 4:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_bulletImpact_04, false);
                break;
            case 5:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_bulletImpact_05, false);
                break;
        }
    }
}
