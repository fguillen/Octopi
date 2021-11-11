using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{

    [SerializeField] float maxTentacleDistance = 5.0f;
    [SerializeField] LayerMask tentacleTargetLayers;

    [SerializeField] public List<TentacleController> tentacles = new List<TentacleController>();

    [SerializeField] ParticleSystem particlesBloodMissile;
    [SerializeField] ParticleSystem particlesBloodBullet;

    [SerializeField] GameObject iconTentaclePrefab;
    GameObject iconTentacle;

    [SerializeField] Transform endScenePosition;
    [SerializeField] GameObject groundColliderObject;
    [SerializeField] SpriteRenderer bodySpriteRenderer;
    [SerializeField] Color flashBodyColor;

    [SerializeField]float electrocutionForce = 50.0f;


    float missileForce = 10.0f;
    float bulletForce = 2.0f;

    Vector2 tentacleHiddenPosition = new Vector2(100, 100);

    Rigidbody2D theRigidBody;

    bool controlsActive;

    void Awake()
    {
        theRigidBody = GetComponent<Rigidbody2D>();
        iconTentacle = Instantiate(iconTentaclePrefab, tentacleHiddenPosition, Quaternion.identity);
        HideIconTentacle();
        controlsActive = true;
    }

    void Update()
    {
        if(controlsActive)
        {
            DrawIconTentacle();

            if(Input.GetMouseButtonDown(0))
            {
                ShootTentacle();
            }

            if(Input.GetMouseButtonDown(1))
            {
                ReleaseTentacle();
            }
        }
    }

    public void SetControlsActive(bool value)
    {
        controlsActive = value;
    }

    public bool GetControlsActive()
    {
        return controlsActive;
    }

    void DrawIconTentacle()
    {
        var result = RaycastTentacle();

        // Gizmos.color = Color.grey;
        // Gizmos.DrawLine(result.rayCastIni, result.rayCastEnd);

        // Debug.Log($"direction: {result.direction}");

        if(result.hit)
        {
            iconTentacle.transform.position = result.hit.collider.gameObject.GetComponent<GrabbableController>().grabbablePosition.transform.position;
        } else
        {
            HideIconTentacle();
        }
    }

    void HideIconTentacle()
    {
        iconTentacle.transform.position = tentacleHiddenPosition;
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

    void ReleaseTentacle()
    {
        var grabbedTentacles = tentacles.Where( e => e.grabbed );

        if(grabbedTentacles.Count() > 0)
        {
            TentacleController tentacle = grabbedTentacles.OrderBy( e => e.lastActivityAt ).First();
            tentacle.Release();
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
        var tentaclesToChoose = FreeTentacles();

        if(tentaclesToChoose.Count() == 0)
            tentaclesToChoose = tentacles;

        TentacleController tentacle = tentaclesToChoose.OrderBy( e => e.lastActivityAt ).First();
        tentacle.StartHook(grabbable);
    }

    public void HitByMissile(Vector2 position, Vector2 direction)
    {
        ParticleSystem particles = Instantiate(particlesBloodMissile, position, Quaternion.identity, transform);
        particles.Play();
        Destroy(particles, 10.0f);
        theRigidBody.AddForce(direction * missileForce, ForceMode2D.Impulse);
    }

    public void HitByBullet(Vector2 position, Vector2 direction)
    {
        ParticleSystem particles = Instantiate(particlesBloodBullet, position, Quaternion.identity, transform);
        particles.Play();
        Destroy(particles, 10.0f);
        theRigidBody.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }


    [ContextMenu("Electrocuted")]
    public void Electrocuted()
    {
        HitByElectrocution(new Vector2(0,0));
    }

    public void HitByElectrocution(Vector2 position)
    {
        ReleaseAllTentacles();
        ParticleSystem particles = Instantiate(particlesBloodMissile, position, Quaternion.identity, transform);
        particles.Play();
        Destroy(particles, 10.0f);
        Vector2 direction;

        if(position.x > transform.position.x)
            direction = new Vector2(-1f, 1f);
        else
            direction = new Vector2(1f, 1f);

        groundColliderObject.GetComponent<Rigidbody2D>().AddForce(direction * electrocutionForce * 0.4f, ForceMode2D.Impulse);
        theRigidBody.AddForce(direction * electrocutionForce, ForceMode2D.Impulse);
        foreach (var tentacle in tentacles)
        {
            tentacle.PushTarget(direction * electrocutionForce * 0.1f);
        }
    }

    public int GrabbedTentaclesCount()
    {
        return tentacles.Where( e => e.grabbed ).Count();
    }

    public List<TentacleController> GrabbedTentacles()
    {
        return tentacles.Where( e => e.grabbed || e.targeting ).ToList();
    }

    public List<TentacleController> FreeTentacles()
    {
        return tentacles.Where( e => !e.grabbed && !e.targeting ).ToList();
    }

    public void MoveToEndScenePosition()
    {
        SetControlsActive(false);
        ReleaseAllTentacles();
        transform.DOMove(endScenePosition.position, 1.0f);
        groundColliderObject.transform.DOMove(endScenePosition.position, 1.0f);
        groundColliderObject.GetComponent<Rigidbody2D>().mass = 100f;
        theRigidBody.mass = 100f;
    }

    public void ReleaseAllTentacles()
    {
        foreach (var tentacle in GrabbedTentacles())
        {
            tentacle.Release();
        }
    }

    public void FlashBodyBackground(float time)
    {
        StartCoroutine(FlashBodyBackgroundCoroutine(time));
    }

    IEnumerator FlashBodyBackgroundCoroutine(float time)
    {
        float untilTime = Time.time + time;
        Color originalBackgroundColor = bodySpriteRenderer.color;

        while(Time.time < untilTime)
        {
            float noiseValue = Mathf.PerlinNoise(Time.time * 10f, 0.0f);
            bodySpriteRenderer.color = noiseValue < 0.5f ? flashBodyColor : Color.white;

            yield return new WaitForEndOfFrame();
        }

        bodySpriteRenderer.color = originalBackgroundColor;
    }
}
