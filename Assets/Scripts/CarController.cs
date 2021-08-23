using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float velocity = 1;
    public RoadPatrolPointController nextPatrolPoint;
    Animator animator;
    public float OriginalZ { get; set; }

    [SerializeField] float degreesToNextPatrolPoint;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.SetBool("Moving", true);
    }

    void Update()
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

        // degreesToNextPatrolPoint = Mathf.Rad2Deg * (Mathf.Atan2(nextPatrolPoint.transform.position.y - transform.position.y, nextPatrolPoint.transform.position.x - transform.position.x));
        // LookTowardsPoint(nextPatrolPoint.transform.position);
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
        Debug.Log($"LookRight() - {degreesToNextPatrolPoint}");
        transform.localScale = new Vector3(-1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ - 0.2f); // Front
    }

    void LookDown()
    {
        Debug.Log($"LookDown() - {degreesToNextPatrolPoint}");
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 90);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ + 0.2f); // Back
    }

    void LookLeft()
    {
        Debug.Log($"LookLeft() - {degreesToNextPatrolPoint}");
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ + 0.2f); // Back
    }

    void LookUp()
    {
        Debug.Log($"LookUp() - {degreesToNextPatrolPoint}");
        transform.localScale = new Vector3(1, -1, 1);
        transform.rotation = Quaternion.Euler(0, 0, -90);
        transform.position = new Vector3(transform.position.x, transform.position.y, OriginalZ - 0.2f); // Front
    }
}
