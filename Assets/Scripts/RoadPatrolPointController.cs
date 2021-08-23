using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPatrolPointController : MonoBehaviour
{
    [SerializeField] public List<RoadPatrolPointController> nextPatrolPoints;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmosSelected()
    {
        // Display the next PatrolPoints
        Gizmos.color = Color.red;
        foreach (var patrolPoint in nextPatrolPoints)
        {
            Gizmos.DrawLine(transform.position, patrolPoint.transform.position);
        }

    }
}
