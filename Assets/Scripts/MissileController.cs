using UnityEngine;

public class MissileController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D TheRigidbody;

    [SerializeField] ParticleSystem particlesExplosion;

    void Awake()
    {
        this.TheRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotateTowardsDirection();
    }

    void RotateTowardsDirection()
    {
        Vector2 moveDirection = TheRigidbody.velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.gameObject.GetComponent<PlayerController>().HitByMissile(collisionInfo.transform.position, TheRigidbody.velocity.normalized);
        }

        Explode();
    }

    void Explode()
    {
        ParticleSystem particles = Instantiate(particlesExplosion, transform.position, Quaternion.identity);
        particles.Play();
        Destroy(particles, 10.0f);
        Destroy(gameObject);
    }
}
