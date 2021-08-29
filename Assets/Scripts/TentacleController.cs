using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoneWrapper
{
    public Transform bone;
    public float originalLength;

    public BoneWrapper(Transform bone)
    {
        this.bone = bone;
        originalLength = bone.localPosition.x;

        Debug.Log($"BoneWrapper.originalLength: {originalLength}");
    }

    public void SetLength(float length)
    {
        bone.localPosition = new Vector2(length, bone.localPosition.y);
        Debug.Log($"BoneWrapper.setLength: {length}");
    }
}

public class TentacleController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerController player;
    [SerializeField] float velocity = 1.0f;

    IEnumerator hookCoroutine;
    SpringJoint2D joint;
    GrabbableController grabbable;
    float tentacleOriginalLength;
    public float lastActivityAt;
    Vector3 originalTargetRelatedPosition;

    [SerializeField] List<Transform> bones;
    List<BoneWrapper> boneWrappers = new List<BoneWrapper>();

    public bool grabbed = false;

    void Awake()
    {
        BuildBoneWrappers();
        CalculateOriginalLength();

        originalTargetRelatedPosition = target.position - transform.position;
    }

    IEnumerator HookCoroutine()
    {
        grabbed = true;

        StretchTentacle(Vector2.Distance(grabbable.transform.position, player.transform.position) * 1.2f);

        if(joint != null)
            Destroy(joint);

        if(grabbable != null)
            grabbable.StopGrab();

        Sequence sequence = DOTween.Sequence();
        // sequence.Append(target.transform.DOMoveY(target.transform.position.y - 5, 0.1f));
        sequence.Append(target.transform.DOMove(grabbable.transform.position, 1f));

        yield return sequence.WaitForCompletion();

        // Maybe released already for other event
        if(grabbable != null)
        {
            CreateJoint(grabbable.transform.position);
            grabbable.StartGrab();
            grabbable.ThrownEvent.AddListener(Release);
        }
    }

    public void Hook(GrabbableController grabbable)
    {
        this.grabbable = grabbable;
        lastActivityAt = Time.time;
        target.GetComponent<Rigidbody2D>().isKinematic = true;
        target.GetComponent<Collider2D>().enabled = false;

        if(hookCoroutine != null)
            StopCoroutine(hookCoroutine);

        hookCoroutine = HookCoroutine();
        StartCoroutine(hookCoroutine);
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
        target.GetComponent<Collider2D>().enabled = true;

        StretchTentacle(tentacleOriginalLength);

        grabbable = null;

        lastActivityAt = Time.time;
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

        Debug.Log($"TentacleController.tentacleOriginalLength: {tentacleOriginalLength}");
    }

    void StretchTentacle(float desiredLength)
    {
        if(desiredLength <= tentacleOriginalLength)
        {
            desiredLength = tentacleOriginalLength;
        }

        float multiplier = desiredLength / tentacleOriginalLength;

        Debug.Log($"TentacleController.tentacleOriginalLength: {tentacleOriginalLength}");
        Debug.Log($"TentacleController.desiredLength: {desiredLength}");
        Debug.Log($"TentacleController.multiplier: {multiplier}");

        foreach (var boneWrapper in boneWrappers)
        {
            boneWrapper.SetLength(boneWrapper.originalLength * multiplier);
        }
    }
}
