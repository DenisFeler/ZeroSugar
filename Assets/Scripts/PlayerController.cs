using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    float horizontalInput;
    public bool foreGround = true;
    public float moveSpeed = 5f;
    bool facingRight = true;
    Vector3 movement;

    //Flashlight Variables
    public Light flashLight;
    bool turnedOff = false;

    //Collision Variables
    private Rigidbody rb;
    private SphereCollider playerCollider;

    //Interaction Variables
    bool canInteract = false;
    bool isDoor = false;
    private Vector3 moveLocation;

    private void Start()
    {
        //Get Player Components on game startup
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        //Closing Eyes interaction
        if (Input.GetMouseButtonDown(0))
        {
            turnedOff = true;
            FlashLight();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            turnedOff = false;
            FlashLight();
        }

        if (canInteract)
        {
            Interaction();
        }

        ForeAndBackgroundMove();
    }

    private void FixedUpdate()
    {
        //Get Left/Right Input (A and D)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //Transfer Movement into Vector
        movement = new Vector3(horizontalInput * moveSpeed, 0f, 0f);
        //PlayerMovement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        //Flip Playermodel depending on Walk direction (Once per)
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    //Change Look Direction of Player by Scale inversion
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    //Moving player from fore and background
    void ForeAndBackgroundMove()
    {
        if (foreGround)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Backmove");
                foreGround = false;
                //Transfer Movement into Vector
                movement = new Vector3(0f, 0f, 200f);
                //PlayerMovement
                rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Frontmove");
                foreGround = true;
                //Transfer Movement into Vector
                movement = new Vector3(0f, 0f, -200f);
                //PlayerMovement
                rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
            }
        }    
    }

    //Eye closing in game
    void FlashLight()
    {
        if (turnedOff)
        {
            flashLight.enabled = false;
        }
        else
        {
            flashLight.enabled = true;
        }
    }

    void Interaction()
    {
        if (isDoor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Moving to the Door given through trigger collision
                gameObject.transform.localPosition = new Vector3(moveLocation.x + 0.85f, moveLocation.y - 0.95f, 23f);
            }
        }
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = true;
            isDoor = true;
            var DoorTo = collision.gameObject.GetComponent<Door>();
            moveLocation = DoorTo.ConnectedPosition;
        }
    }

    private void OnTriggerStay (Collider collision)
    {
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = true;
            isDoor = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = false;
            isDoor = false;
        }
    }
}
