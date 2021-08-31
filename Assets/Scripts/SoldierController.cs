using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class SoldierController : Shooter
{
    public float velocity = 1;
    public RoadPatrolPointController nextPatrolPoint;
    Animator animator;
    public float OriginalZ { get; set; }
    PlayerController player;

    [SerializeField] float betweenShootsTime = 2.0f;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform barrelEnd;
    [SerializeField] float shootForce = 1.0f;
    [SerializeField] Vector3 shootingOffset = new Vector3(0, 1, 0);
    [SerializeField] int numBulletsPerShoot = 3;

    float nextShootAt;
    bool idle = false;
    bool inRange = false;
    Quaternion gunReleaseRotation;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("/PlayerGame/Player").GetComponent<PlayerController>();
        Debug.Assert(player != null);
        GameManagerController.Instance.IncreaseSoldiers();
    }

    void OnDestroy()
    {
        GameManagerController.Instance.DecreaseSoldiers();
    }

    void Start()
    {
        animator.SetBool("Moving", true);
        nextShootAt = Time.time + Utils.AddNoise(betweenShootsTime);
        gunReleaseRotation = gun.localRotation;
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
        Vector3 nextPatrolPointPositionWithCustomZ = new Vector3(nextPatrolPoint.transform.position.x, nextPatrolPoint.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, nextPatrolPointPositionWithCustomZ, velocity * Time.deltaTime);

        if(Vector3.Distance(transform.position, nextPatrolPointPositionWithCustomZ) < 0.01)
        {
            if(nextPatrolPoint.nextPatrolPoints.Count == 0)
            {
                Destroy(gameObject);
            } else
            {
                NextPatrolPointClosestToPlayer();
            }
        }
    }

    // void TargetPlayer()
    // {
    //    gun.rotation = Utils.Rotation2DTowards(gun, player.transform.position, transform.localScale.x < 0);
    // }


    IEnumerator ShootCoroutine(){
        idle = true;
        animator.SetBool("Moving", false);

        // Point towards Player
        Tween tween = gun.DORotateQuaternion(Utils.Rotation2DTowards(gun.transform, player.transform.position, transform.localScale.x < 0), 0.5f);
        yield return tween.WaitForCompletion();

        int numBullets = Utils.AddNoise(numBulletsPerShoot);
        for (int i = 0; i < numBullets; i++)
        {
            Shoot();
            if(i + 1 < numBullets)
                yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        // Return gun to release position
        tween = gun.DOLocalRotateQuaternion(gunReleaseRotation, 0.8f);
        yield return tween.WaitForCompletion();


        nextShootAt = Time.time + Utils.AddNoise(betweenShootsTime);
        animator.SetBool("Moving", true);
        idle = false;
    }

    void OnDrawGizmos()
    {
        Vector2 direction = (player.transform.position + shootingOffset - gun.position).normalized;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(barrelEnd.position, (Vector2)barrelEnd.position + (direction * shootForce));
    }

    void Shoot()
    {
        // Debug.Log("Shoot!!");
        GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, Quaternion.identity);
        Vector2 direction = (player.transform.position + shootingOffset - gun.position).normalized;
        bullet.GetComponent<BulletController>().TheRigidbody.AddForce(direction * shootForce, ForceMode2D.Impulse);
    }

    void NextPatrolPointClosestToPlayer()
    {
        NextPatrolPoint(nextPatrolPoint.nextPatrolPoints.OrderBy( e => Vector2.Distance(e.transform.position, player.transform.position) ).ToList()[0]);
    }

    public void NextPatrolPoint(RoadPatrolPointController patrolPoint)
    {
        this.nextPatrolPoint = patrolPoint;
        LookTowardsPoint(nextPatrolPoint.transform.position);
    }

    void LookTowardsPoint(Vector3 point)
    {
        // Vector2.Angle doesn't work
        float degreesToNextPatrolPoint = Mathf.Rad2Deg * (Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x));

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

    public override void InRange(){
        this.inRange = true;
    }

    public override void OutOfRange(){
        this.inRange = false;
    }
}
