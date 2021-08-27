using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GrabbableController : MonoBehaviour
{
    [SerializeField] Rigidbody2D theRigidbody;
    [SerializeField] Transform scalable;
    [SerializeField] Transform shakeable;
    [SerializeField] SpriteRenderer colorizable;
    [SerializeField] Transform mainObject;

    [SerializeField] float timeToThrow = 2.0f;
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] bool jumpUp;

    Color originalColor;
    [SerializeField] Color activeColor = new Color(0.837667f, 0.376246f, 0.8962264f);

    PlayerController player;
    IEnumerator grabCoroutine;
    Tween shakeTween;

    Vector3 originalScale;
    Vector3 originalPosition;

    public UnityEvent StartGrabEvent;
    public UnityEvent StopGrabEvent;
    public UnityEvent ThrownEvent;

    bool thrown = false;

    void Awake()
    {
        player = GameObject.Find("/PlayerWrapper/Player").GetComponent<PlayerController>();
        Debug.Assert(player != null);

        originalScale = scalable.localScale;
        originalPosition = shakeable.localPosition;
        originalColor = colorizable.color;
    }

    public void StartGrab()
    {
        Debug.Log("GrabbableController.StartGrab()");
        StartGrabEvent.Invoke();

        grabCoroutine = GrabCoroutine();
        StartCoroutine(grabCoroutine);
    }

    public void StopGrab()
    {
        StopGrabEvent.Invoke();

        StopShake();
        colorizable.color = originalColor;

        if(grabCoroutine != null)
            StopCoroutine(grabCoroutine);
    }

    void StartShake()
    {
        shakeTween = shakeable.DOShakePosition(100, new Vector3(0.05f, 0.05f, 0), 20, 90, false, false);
    }

    void StopShake()
    {
        if(shakeTween != null)
            shakeTween.Kill();

        shakeable.localPosition = originalPosition;
    }

    IEnumerator GrabCoroutine()
    {
        colorizable.color = activeColor;
        StartShake();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scalable.DOScale(originalScale * 0.95f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(scalable.DOScale(originalScale, 0.1f));

        yield return new WaitForSeconds(Utils.AddNoise(timeToThrow));
        StopShake();

        ThrowToPlayer();
    }

    void ThrowToPlayer()
    {
        ThrownEvent.Invoke();

        colorizable.color = originalColor;
        thrown = true;

        mainObject.position = new Vector3(mainObject.position.x, mainObject.position.y, -1.0f); // on Front
        theRigidbody.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direction = (player.transform.position - scalable.position).normalized;

        if(jumpUp)
        {
            direction += Vector2.up;
            direction = direction.normalized;
        }

        theRigidbody.AddForce(direction * throwForce, ForceMode2D.Impulse);
        theRigidbody.AddTorque(Random.Range(-throwForce, throwForce), ForceMode2D.Impulse);
    }

    void OnMouseDown()
    {
        if(!thrown)
            StartGrab();
    }

    void OnMouseUp()
    {
        if(!thrown)
            StopGrab();
    }
}
