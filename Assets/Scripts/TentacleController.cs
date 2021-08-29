using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerController player;

    SpringJoint2D joint;
    GrabbableController grabbable;

    bool grabbed = false;

    public void Hook(GrabbableController grabbable)
    {
        target.position = grabbable.transform.position;

        this.grabbable = grabbable;
        this.grabbable.StartGrab();

        if(joint != null)
            Destroy(joint);

        joint = player.gameObject.AddComponent<SpringJoint2D>();
        joint.enableCollision = true;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grabbable.transform.position;
        joint.distance = 1.25f;
        joint.dampingRatio = 0f;
        joint.frequency = 1f;


        grabbed = true;
    }
}
