using UnityEngine;


public class PlayerMovementsController : MonoBehaviour
{

    [SerializeField] PlayerController player;
    Rigidbody2D theRigidBody;
    [SerializeField] float movementForce;
    [SerializeField] float jumpForce;

    void Awake()
    {
        theRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        int grabbedTentaclesCount = player.GrabbedTentaclesCount();
        if(grabbedTentaclesCount > 0)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft(grabbedTentaclesCount);
            }

            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight(grabbedTentaclesCount);
            }

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MoveUp(grabbedTentaclesCount);
            }
        }

        // if(grabbedTentaclesCount == 4)
        // {
        //     if(Input.GetButtonDown("Jump"))
        //     {
        //         Jump();
        //     }
        // }


        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void MoveLeft(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.left * movementForce * forceMultiplier, ForceMode2D.Force);
    }

    void MoveRight(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.right * movementForce * forceMultiplier, ForceMode2D.Force);
    }

    void MoveUp(int forceMultiplier)
    {
        theRigidBody.AddForce(Vector2.up * movementForce * forceMultiplier, ForceMode2D.Force);
    }

    void Jump()
    {
        player.ReleaseAllTentacles();
        theRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
