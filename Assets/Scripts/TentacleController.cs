using System.Collections;
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

    SpringJoint2D joint;
    GrabbableController grabbable;
    float tentacleOriginalLength;

    [SerializeField] List<Transform> bones;
    List<BoneWrapper> boneWrappers = new List<BoneWrapper>();

    bool grabbed = false;

    void Awake()
    {
        BuildBoneWrappers();
        CalculateOriginalLength();
    }

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

        StretchTentacle(Vector2.Distance(target.position, player.transform.position) * 1.2f);

        grabbed = true;
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
