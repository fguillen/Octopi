using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityCore.Audio;

public class HelicopterController : Shooter
{
    public float velocity = 1;
    public Vector3 nextPatrolPoint;
    Animator animator;
    public float OriginalZ { get; set; }
    PlayerController player;

    [SerializeField] float betweenShootsTime = 2.0f;
    [SerializeField] Transform gun;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform barrelEnd;
    [SerializeField] float shootForce = 1.0f;
    [SerializeField] Vector3 shootingOffset = new Vector3(0, 1, 0);
    [SerializeField] float distanceToPlayer = 4.0f;

    float nextShootAt;
    bool idle = false;
    bool inRange = false;
    Quaternion gunReleaseRotation;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("/PlayerGame/Player").GetComponent<PlayerController>();
        Debug.Assert(player != null);
        GameManagerController.Instance.IncreaseHelicopters(this);
    }

    void OnDestroy()
    {
        GameManagerController.Instance.DecreaseHelicopters(this);
    }

    void Start()
    {
        animator.SetBool("Moving", true);
        nextShootAt = Time.time + Utils.AddNoise(betweenShootsTime);
        gunReleaseRotation = gun.localRotation;
        NextPatrolPointCloseToPlayer();
    }

    void Update()
    {

        if(!idle)
            Move();

        if(!idle && inRange && nextShootAt <= Time.time && GameManagerController.Instance.CanShoot())
            StartCoroutine(ShootCoroutine());

        // TargetPlayer();
    }

    void Move()
    {
        Vector3 nextPatrolPointPositionWithCustomZ = new Vector3(nextPatrolPoint.x, nextPatrolPoint.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, nextPatrolPointPositionWithCustomZ, velocity * Time.deltaTime);

        if(Vector3.Distance(transform.position, nextPatrolPointPositionWithCustomZ) < 0.01)
        {
            if(GameManagerController.Instance.EndGame())
            {
                NextPatrolPointFarToPlayer();
            } else
            {
                NextPatrolPointCloseToPlayer();
            }
        }
    }


    IEnumerator ShootCoroutine(){
        idle = true;
        animator.SetBool("Moving", false);

        // Point towards Player
        Tween tween = gun.DORotateQuaternion(Utils.Rotation2DTowards(gun.transform, player.transform.position, transform.localScale.x < 0), 2.0f);

        // TODO: calculate this depending in the size of the angle between the actual rotation and the desired rotation
        float soundSpeed = Utils.AddNoise(0.5f);
        while(tween.IsPlaying())
        {
            // AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_tankTurret, false);
            yield return new WaitForSeconds(soundSpeed);
        }

        Shoot();

        yield return new WaitForSeconds(1f);

        // Return gun to release position
        tween = gun.DOLocalRotateQuaternion(gunReleaseRotation, 2.0f);

        // TODO: calculate this depending in the size of the angle between the actual rotation and the desired rotation
        while(tween.IsPlaying())
        {
            // AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_tankTurret, false);
            yield return new WaitForSeconds(soundSpeed);
        }

        nextShootAt = Time.time + Utils.AddNoise(betweenShootsTime);
        animator.SetBool("Moving", true);
        idle = false;
    }

    void Shoot()
    {
        // Debug.Log("Shoot!!");
        GameObject missile = Instantiate(missilePrefab, barrelEnd.position, Quaternion.identity);
        Vector2 direction = (player.transform.position + shootingOffset - gun.position).normalized;
        missile.GetComponent<MissileController>().TheRigidbody.AddForce(direction * shootForce, ForceMode2D.Impulse);

        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_missileShoot, false);
    }

    public void NextPatrolPointCloseToPlayer()
    {
        Vector3 point = Utils.PositionInCircumference(player.transform.position, Utils.AddNoise(distanceToPlayer, 5.0f), Random.Range(15, 165));
        NextPatrolPoint(point);
    }

    public void NextPatrolPointFarToPlayer()
    {
        Vector3 point = Utils.PositionInCircumference(player.transform.position, 20.0f, Random.Range(15, 165));
        NextPatrolPoint(point);
    }

    void NextPatrolPoint(Vector3 patrolPoint)
    {
        this.nextPatrolPoint = patrolPoint;
        LookTowardsPoint(nextPatrolPoint);
    }

    void LookTowardsPoint(Vector3 point)
    {
        if(transform.position.x < point.x)
        {
            LookRight();
        } else
        {
            LookLeft();
        }
    }

    void LookRight()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }


    void LookLeft()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public override void InRange(){
        this.inRange = true;
    }

    public override void OutOfRange(){
        this.inRange = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nextPatrolPoint, 0.8f);
    }

    public void GoAwayFromPlayer()
    {
        NextPatrolPointFarToPlayer();
    }
}
