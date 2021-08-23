using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float velocity = 1;
    public RoadPatrolPointController nextPatrolPoint;
    Animator animator;

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

        if(
            (velocity > 0 && transform.localScale.x > 0) ||
            (velocity < 0 && transform.localScale.x < 0)
        )
        {
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

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
        Vector2 dir = transform.position - nextPatrolPoint.transform.position;
        transform.right = dir;
    }
}
