using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float maxTentacleDistance = 5.0f;
    [SerializeField] List<Transform> tentacleTargets;
    [SerializeField] LayerMask tentacleTargetLayers;

    // [SerializeField] int numPoints = 3;
    // [SerializeField] float radius = 1.0f;

    // void OnDrawGizmos()
    // {
    //     if(numPoints > 1)
    //     {
    //         float minDegrees = 15.0f;
    //         float maxDegrees = 165.0f;
    //         float degreesStep = (maxDegrees - minDegrees) / (numPoints - 1);

    //         for (int i = 0; i < numPoints; i++)
    //         {
    //             float degrees = minDegrees + (i * degreesStep);
    //             Vector3 position = Utils.PositionInCircumference(transform.position, radius, degrees);
    //             Gizmos.color = Color.red;
    //             Gizmos.DrawWireSphere(position, 0.5f);
    //         }
    //     }
    // }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShootTentacle();
        }
    }

    void OnDrawGizmos()
    {
        var result = RaycastTentacle();

        Gizmos.color = Color.grey;
        Vector2 finalPosition =  (Vector2)transform.position + (result.direction * maxTentacleDistance);
        Gizmos.DrawLine(transform.position, finalPosition);

        // Debug.Log($"direction: {result.direction}");

        if(result.hit)
        {
            Gizmos.DrawSphere(result.hit.transform.position, 0.5f);
        }
    }

    void ShootTentacle()
    {
        RaycastHit2D hit = RaycastTentacle().hit;

        if(hit)
        {
            hit.collider.gameObject.GetComponent<GrabbableController>().StartGrab();
        }
    }

    (Vector2 direction, RaycastHit2D hit) RaycastTentacle()
    {
        Vector3 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxTentacleDistance, tentacleTargetLayers);

        return (direction: direction, hit: hit);
    }
}
