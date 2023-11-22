using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    float horizontalInput;
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
    private int RoomNum;

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
    }

    private void FixedUpdate()
    {
        //Get Left/Right Input (A and D)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (turnedOff)
        {
            //Transfer Movement into Vector
            movement = new Vector3(horizontalInput * moveSpeed / 8f, 0f, 0f);
        }
        else
        {
            //Transfer Movement into Vector
            movement = new Vector3(horizontalInput * moveSpeed, 0f, 0f);
        }

        //Flip Playermodel depending on Walk direction (Once per)
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        //PlayerMovement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    //Change Look Direction of Player by Scale inversion
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
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
                
            
                switch (RoomNum)
                {
                    case 0:
                        //Positionierung des Spielers nach interagieren mit Tür
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 0.95f, moveLocation.z);
                        break;
                    case 1:
                        //Positionierung des Spielers nach interagieren mit Tür
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 0.95f, moveLocation.z);
                        break;
                    case 2:
                        //Positionierung des Spielers nach interagieren mit Tür
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 0.85f, moveLocation.y - 0.95f, moveLocation.z - 3.25f);
                        break;
                    case 3:
                        //Positionierung des Spielers nach interagieren mit Tür
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 0.85f, moveLocation.y - 0.95f, moveLocation.z - 3.25f);
                        break;
                    default:
                        Debug.Log("No Room under that number existant");
                        break;
                }
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
            RoomNum = (int)DoorTo.toRoom;
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
