using System.Collections.Generic;
using UnityEngine;

public class BoneWrapper
{
    public Transform bone;
    public float originalLength;

    public BoneWrapper(Transform bone)
    {
        this.bone = bone;
        originalLength = bone.localPosition.x;

        // Debug.Log($"BoneWrapper.originalLength: {originalLength}");
    }

    public void SetLength(float length)
    {
        bone.localPosition = new Vector2(length, bone.localPosition.y);
        // Debug.Log($"BoneWrapper.setLength: {length}");
    }
}

public class TentacleController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] PlayerController player;
    [SerializeField] float velocity = 1.0f;


    Collider2D targetCollider;
    Rigidbody2D targetRigidbody;
    SpringJoint2D joint;
    GrabbableController grabbable;
    float tentacleOriginalLength;
    public float lastActivityAt;
    Vector3 originalTargetRelatedPosition;

    [SerializeField] List<Transform> bones;
    List<BoneWrapper> boneWrappers = new List<BoneWrapper>();

    public bool grabbed = false;
    public bool targeting = false;

    void Awake()
    {
        BuildBoneWrappers();
        CalculateOriginalLength();

        originalTargetRelatedPosition = target.transform.position - transform.position;

        targetCollider = target.GetComponent<Collider2D>();
        targetRigidbody = target.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        LiberateIfHolding();

        if(targeting)
            MoveTowardsGrabbed();
    }

    void MoveTowardsGrabbed()
    {
        target.transform.position = Vector2.MoveTowards(target.transform.position, grabbable.grabbablePosition.transform.position, velocity * Time.deltaTime);
        float distanceToGrabbable = Vector2.Distance(player.transform.position, grabbable.grabbablePosition.transform.position);

        // If it went too far Release
        if(distanceToGrabbable > 6f)
        {
            Release();
        } else
        {
            if(Vector2.Distance(target.transform.position, grabbable.grabbablePosition.transform.position) < 0.01f)
                Hook();
        }
    }

    void LiberateIfHolding()
    {
        if(target.transform.position.y > player.transform.position.y)
        {
            targetCollider.enabled = false;
        } else {
            targetCollider.enabled = true;
        }
    }

    public void StartHook(GrabbableController grabbable)
    {
        targeting = true;
        grabbed = false;

        if(joint != null)
            Destroy(joint);

        if(grabbable != null && player.GrabbedTentacles().Count == 0)
            grabbable.StopGrab();

        this.grabbable = grabbable;
        lastActivityAt = Time.time;
        target.GetComponent<Rigidbody2D>().isKinematic = true;
        target.GetComponent<Rigidbody2D>().simulated = false;
        target.GetComponent<Collider2D>().enabled = false;

        StretchTentacle(Vector2.Distance(player.transform.position, grabbable.grabbablePosition.transform.position) * 1.2f);
    }

    void Hook()
    {
        CreateJoint(grabbable.grabbablePosition.transform.position);
        grabbable.StartGrab();
        grabbable.ThrownEvent.AddListener(Release);
        targeting = false;
        grabbed = true;
    }

    void CreateJoint(Vector2 position)
    {
        joint = player.gameObject.AddComponent<SpringJoint2D>();
        joint.enableCollision = true;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = position;
        joint.distance = 1.25f;
        joint.dampingRatio = 0f;
        joint.frequency = 1f;
    }

    public void Release()
    {
        if(joint != null)
            Destroy(joint);

        if(grabbable != null)
            grabbable.StopGrab();

        // target.transform.DOMove(transform.position + originalTargetRelatedPosition, 1.0f);
        target.GetComponent<Rigidbody2D>().isKinematic = false;
        target.GetComponent<Rigidbody2D>().simulated = true;
        target.GetComponent<Collider2D>().enabled = true;

        StretchTentacle(tentacleOriginalLength);

        grabbable = null;

        lastActivityAt = Time.time;

        targeting = false;
        grabbed = false;
    }

    void BuildBoneWrappers()
    {
        foreach (var bone in bones)
        {
            BoneWrapper boneWrapper = new BoneWrapper(bone);
            boneWrappers.Add(boneWrapper);
        }
    }

    void CalculateOriginalLength()
    {
        float length = 0;

        foreach (var boneWrapper in boneWrappers)
        {
            length += boneWrapper.originalLength;
        }

        this.tentacleOriginalLength = length;

        // Debug.Log($"TentacleController.tentacleOriginalLength: {tentacleOriginalLength}");
    }

    void StretchTentacle(float desiredLength)
    {
        if(desiredLength <= tentacleOriginalLength)
        {
            desiredLength = tentacleOriginalLength;
        }

        float multiplier = desiredLength / tentacleOriginalLength;

        // Debug.Log($"TentacleController.tentacleOriginalLength: {tentacleOriginalLength}");
        // Debug.Log($"TentacleController.desiredLength: {desiredLength}");
        // Debug.Log($"TentacleController.multiplier: {multiplier}");

        foreach (var boneWrapper in boneWrappers)
        {
            boneWrapper.SetLength(boneWrapper.originalLength * multiplier);
        }
    }

    public void Jump(float force)
    {
        targetRigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public void PushTarget(Vector2 force)
    {
        targetRigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}
