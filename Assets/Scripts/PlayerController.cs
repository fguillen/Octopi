using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] float maxTentacleDistance = 5.0f;
    [SerializeField] LayerMask tentacleTargetLayers;

    [SerializeField] List<TentacleController> tentacles = new List<TentacleController>();

    [SerializeField] ParticleSystem particlesBloodMissile;
    [SerializeField] ParticleSystem particlesBloodBullet;

    [SerializeField] GameObject iconTentaclePrefab;
    GameObject iconTentacle;

    float missileForce = 10.0f;
    float bulletForce = 2.0f;
    Vector2 tentacleHiddenPosition = new Vector2(100, 100);

    Rigidbody2D theRigidBody;

    void Awake()
    {
        theRigidBody = GetComponent<Rigidbody2D>();
        iconTentacle = Instantiate(iconTentaclePrefab, tentacleHiddenPosition, Quaternion.identity);
        HideIconTentacle();
    }

    void Update()
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

    void DrawIconTentacle()
    {
        var result = RaycastTentacle();

        // Gizmos.color = Color.grey;
        // Gizmos.DrawLine(result.rayCastIni, result.rayCastEnd);

        // Debug.Log($"direction: {result.direction}");

        if(result.hit)
        {
            iconTentacle.transform.position = result.hit.transform.position;
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
        var grabbedTentacles = tentacles.Where(e => e.grabbed);

        if(grabbedTentacles.Count() > 0)
        {
            TentacleController tentacle = grabbedTentacles.OrderBy( e => e.lastActivityAt).First();
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
        TentacleController tentacle = tentacles.OrderBy( e => e.lastActivityAt ).First();
        tentacle.Hook(grabbable);
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

    public int GrabbedTentacles()
    {
        return tentacles.Where(e => e.grabbed).Count();
    }
}
