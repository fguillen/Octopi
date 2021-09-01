using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityCore.Audio;

public class CarController : MonoBehaviour
{
    public float velocity = 1;
    public RoadPatrolPointController nextPatrolPoint;
    Animator animator;
    public float OriginalZ { get; set; }

    [SerializeField] float degreesToNextPatrolPoint;
    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesFire;

    [SerializeField] Color burntColor = new Color(0.27f, 0.076f, 0.15f);
    [SerializeField] SpriteRenderer colorizable;
    [SerializeField] Collider2D theCollider;

    bool idle = false;
    bool onRoad = true;
    bool exploded = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        GameManagerController.Instance.IncreaseCars();
    }

    void OnDestroy()
    {
        GameManagerController.Instance.DecreaseCars();
    }

    void Start()
    {
        animator.SetBool("Moving", true);
    }

    void Update()
    {
        if(!idle)
            Move();


    }

    void Move()
    {
        Vector3 nextPatrolPointPositionWithCustomZ = new Vector3(nextPatrolPoint.transform.position.x, nextPatrolPoint.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, nextPatrolPointPositionWithCustomZ, velocity * Time.deltaTime);

        if(Vector3.Distance(transform.position, nextPatrolPointPositionWithCustomZ) < 0.01)
        {
            if(nextPatrolPoint.nextPatrolPoints.Count == 0)
            {
                Destroy(gameObject);
            } else
            {
                NextPatrolPoint(nextPatrolPoint.nextPatrolPoints[Random.Range(0, nextPatrolPoint.nextPatrolPoints.Count)]);
            }
        }
    }

    public void NextPatrolPoint(RoadPatrolPointController patrolPoint)
    {
        this.nextPatrolPoint = patrolPoint;
        LookTowardsPoint(nextPatrolPoint.transform.position);
    }

    void LookTowardsPoint(Vector3 point)
    {
        // Vector2.Angle doesn't work
        degreesToNextPatrolPoint = Mathf.Rad2Deg * (Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x));

        if(
            (degreesToNextPatrolPoint < 45 && degreesToNextPatrolPoint >= 0) ||
            (degreesToNextPatrolPoint < 0 && degreesToNextPatrolPoint >= -45)
        )
        {
            LookRight();
        } else if (degreesToNextPatrolPoint < -45 && degreesToNextPatrolPoint >= -135)
        {
            LookDown();
        } else if (
            (degreesToNextPatrolPoint < -135 && degreesToNextPatrolPoint >= -180) ||
            (degreesToNextPatrolPoint <= 180 && degreesToNextPatrolPoint >= 135)
        )
        {
            LookLeft();
        } else if (degreesToNextPatrolPoint >= 45 && degreesToNextPatrolPoint < 135)
        {
            LookUp();
        }
    }

    void LookRight()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ - 0.2f); // Front
    }

    void LookDown()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 90);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ + 0.2f); // Back
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ + 0.2f); // Back
    }

    void LookUp()
    {
        transform.localScale = new Vector3(1, -1, 1);
        transform.rotation = Quaternion.Euler(0, 0, -90);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ - 0.2f); // Front
    }

    public void StartGrab()
    {
        // Debug.Log("CarController.StartGrab()");
        animator.SetBool("Moving", false);
        idle = true;
    }

    public void StopGrab()
    {
        Debug.Log("CarController.StopGrab()");
        if(onRoad)
        {
            animator.SetBool("Moving", true);
            idle = false;
        }
    }

    public void Thrown()
    {
        Debug.Log("CarController.Thrown()");
        animator.SetBool("Moving", true);
        idle = true;
        onRoad = false;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("CarGround"))
    //     {
    //         Explode();
    //     }
    // }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(!exploded &&collisionInfo.gameObject.CompareTag("CarGround"))
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
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_fire, false);

        yield return colorizable.DOColor(burntColor, Utils.AddNoise(10.0f)).WaitForCompletion();

        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.SFX_fire, true);
        animator.SetBool("Moving", false);
    }

    void SoundCrashPlay()
    {
        int carCrashClipClip = Random.Range(1, 4);
        switch (carCrashClipClip)
        {
            case 1:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_carCrash_01, false);
                break;
            case 2:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_carCrash_02, false);
                break;
            case 3:
                AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_carCrash_03, false);
                break;
        }
    }
}
