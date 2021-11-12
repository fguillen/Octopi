using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour
{
    public float velocity = 1;
    public RoadPatrolPointController nextPatrolPoint;
    RoadPatrolPointController previousPatrolPoint;
    Animator animator;

    [SerializeField] float pauseTime = 2.0f;
    [SerializeField] float pauseEachSeconds = 10.0f;


    float nextPauseAt;

    bool idle = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        GameManagerController.Instance.IncreasePeople(this);
    }

    void OnDestroy()
    {
        GameManagerController.Instance.DecreasePeople(this);
    }

    void Start()
    {
        animator.SetBool("Moving", true);
        nextPauseAt = Time.time + Utils.AddNoise(pauseEachSeconds);
    }

    void Update()
    {
        if(!idle)
        {
            Move();
            CheckPause();
        }
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
                NextPatrolPoint();
            }
        }
    }

    void CheckPause()
    {
        if(nextPauseAt <= Time.time)
            StartCoroutine(ChangeDirectionCoroutine());

    }

    public void NextPatrolPoint()
    {
        NextPatrolPoint(nextPatrolPoint.nextPatrolPoints[Random.Range(0, nextPatrolPoint.nextPatrolPoints.Count)]);
    }

    public void NextPatrolPoint(RoadPatrolPointController patrolPoint)
    {
        if(this.nextPatrolPoint == null)
            this.previousPatrolPoint = patrolPoint;
        else
            this.previousPatrolPoint = this.nextPatrolPoint;

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
    }

    void LookDown()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void LookUp()
    {
        transform.localScale = new Vector3(1, -1, 1);
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    IEnumerator ChangeDirectionCoroutine()
    {
        idle = true;
        animator.SetBool("Moving", false);

        yield return new WaitForSeconds(Utils.AddNoise(pauseTime, pauseTime));

        RoadPatrolPointController[] possiblePatrolPoints = { previousPatrolPoint, nextPatrolPoint };
        NextPatrolPoint(possiblePatrolPoints[Random.Range(0, possiblePatrolPoints.Length)]);
        this.nextPauseAt = Time.time + Utils.AddNoise(pauseEachSeconds);
        animator.SetBool("Moving", true);
        idle = false;
    }
}
