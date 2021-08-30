using UnityEngine;


public class PlayerMovementsController : MonoBehaviour
{

    [SerializeField] PlayerController player;
    Rigidbody2D theRigidBody;
    [SerializeField] float force;

    void Awake()
    {
        theRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        int grabbedTentacles = player.GrabbedTentacles();
        if(grabbedTentacles > 0)
        {
            if(Input.GetKey(KeyCode.A))
            {
                MoveLeft(grabbedTentacles);
            }

            if(Input.GetKey(KeyCode.D))
            {
                MoveRight(grabbedTentacles);
            }

            if(Input.GetKey(KeyCode.W))
            {
                MoveUp(grabbedTentacles);
            }
        }
    }

    void MoveLeft(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.left * force * forceMultiplier, ForceMode2D.Force);
    }

    void MoveRight(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.right * force * forceMultiplier, ForceMode2D.Force);
    }

    void MoveUp(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.up * force * forceMultiplier, ForceMode2D.Force);
    }
}
