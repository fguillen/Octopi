using System.Collections;

using UnityEngine;
using DG.Tweening;

public class GrabbableController : MonoBehaviour
{
    [SerializeField] Rigidbody2D theRigidbody;
    [SerializeField] Animator animator;
    [SerializeField] Transform scalable;
    [SerializeField] Transform shakeable;

    [SerializeField] float timeToShake = 2.0f;
    PlayerController player;
    IEnumerator grabCoroutine;
    Tween shakeTween;

    void Awake()
    {
        player = GameObject.Find("/PlayerWrapper/Player").GetComponent<PlayerController>();
        Debug.Assert(player != null);
    }

    public void StartGrab()
    {
        if(animator != null)
            animator.enabled = false;

        grabCoroutine = GrabCoroutine();
        StartCoroutine(grabCoroutine);
    }

    public void StopGrab()
    {
        StopShake();

        if(grabCoroutine != null)
            StopCoroutine(grabCoroutine);
    }

    void StartShake()
    {
        shakeTween = shakeable.DOShakePosition(100, new Vector3(0.1f, 0.1f, 0), 10);
    }

    void StopShake()
    {
        if(shakeTween != null)
            shakeTween.Kill();
    }

    IEnumerator GrabCoroutine()
    {
        scalable.DOScale(scalable.localScale * 0.8f, 0.5f).SetEase(Ease.OutElastic).OnComplete(StartShake);
        yield return new WaitForSeconds(timeToShake);
        StopShake();

        ThrowToPlayer();
    }

    void ThrowToPlayer()
    {
        theRigidbody.bodyType = RigidbodyType2D.Dynamic;
        theRigidbody.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
    }

    void OnMouseDown()
    {
        StartGrab();
    }

    void OnMouseUp()
    {
        StopGrab();
    }
}
