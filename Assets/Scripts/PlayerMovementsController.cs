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
        int grabbedTentacles = player.GrabbedTentaclesCount();
        if(grabbedTentacles > 0)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft(grabbedTentacles);
            }

            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight(grabbedTentacles);
            }

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
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
