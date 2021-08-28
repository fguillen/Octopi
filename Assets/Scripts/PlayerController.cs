using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float maxTentacleDistance = 5.0f;
    [SerializeField] List<Transform> tentacleTargets;
    [SerializeField] LayerMask tentacleTargetLayers;



    List<SpringJoint2D> joints = new List<SpringJoint2D>();

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
        Gizmos.DrawLine(result.rayCastIni, result.rayCastEnd);

        // Debug.Log($"direction: {result.direction}");

        if(result.hit)
        {
            Gizmos.DrawSphere(result.hit.transform.position, 0.1f);
        }
    }

    void ShootTentacle()
    {
        RaycastHit2D hit = RaycastTentacle().hit;

        if(hit)
        {
            HookToGrabbable(hit.collider.gameObject.GetComponent<GrabbableController>());
        }
    }

    (Vector2 rayCastIni, Vector2 rayCastEnd, RaycastHit2D hit) RaycastTentacle()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        float distanceToMousePosition = Vector2.Distance(transform.position, mousePosition);

        Vector2 rayCastIni;
        Vector2 rayCastEnd;
        float rayCastDistance;

        if(distanceToMousePosition <= maxTentacleDistance)
        {
            rayCastIni = mousePosition;
            rayCastEnd = mousePosition + (direction * (maxTentacleDistance - distanceToMousePosition));
            rayCastDistance = maxTentacleDistance - distanceToMousePosition;
        } else
        {
            rayCastIni = (Vector2)transform.position + (direction * maxTentacleDistance);
            rayCastEnd = transform.position;
            rayCastDistance = maxTentacleDistance;
            direction = -direction;
        }

        RaycastHit2D hit = Physics2D.Raycast(rayCastIni, direction, rayCastDistance, tentacleTargetLayers);

        return (rayCastIni: rayCastIni, rayCastEnd: rayCastEnd, hit: hit);
    }

    void HookToGrabbable(GrabbableController grabbable)
    {
        grabbable.StartGrab();

        if(joints.Count >= 4)
        {
            Destroy(joints[0]);
            joints.RemoveAt(0);
        }

        SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grabbable.transform.position;

        float distanceFromPoint = Vector2.Distance(transform.position, grabbable.transform.position);

        joint.distance = 1.25f;
        joint.dampingRatio = 0f;
        joint.frequency = 1f;

        joints.Add(joint);
    }


}
