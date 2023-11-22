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

    //Collision Variables
    private Rigidbody rb;
    private SphereCollider playerCollider;

    //Interaction Variables
    public bool canInteract = false;
    bool isDoor = false;
    private Vector3 moveLocation;
    private int RoomNum;

    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;

    //Nightlight Variables
    private GameObject nightLight;
    private bool isNightlight = false;
    public bool hasNightlight = false;

    private void Start()
    {
        //Get Player Components on game startup
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();

        //Get Flashlight Components on game startup
        flashLight = GameObject.FindGameObjectWithTag("Flashlight");
        flc = flashLight.gameObject.GetComponent<FlashlightController>();
    }

    private void Update()
    {
        //Check for interactable objects encountered
        if (canInteract)
        {
            Interaction();
        }
    }

    private void FixedUpdate()
    {
        //Get Left/Right Inputs from Unity internal inputs (A and D)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Checks for Flashlight state
        if (flc.turnedOn)
        {
            //Transfer Movement into Vector
            movement = new Vector3(horizontalInput * moveSpeed, 0f, 0f);   
        }
        else
        {
            //Transfer Movement into Vector and slow it down 
            movement = new Vector3(horizontalInput * moveSpeed / 8f, 0f, 0f);
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

        //Player movement
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

    //Interactable World Objects
    void Interaction()
    {
        if (isDoor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Moving to the Door given through trigger collision
                switch (RoomNum)
                {
                    case 0: //Entering Child Bedroom from Floor
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 1: //Entering Floor from Child Bedroom
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 2: //Currently not used
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 0.85f, moveLocation.y - 0.95f, moveLocation.z - 3.25f);
                        break;
                    case 3: //Currently not used
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 0.85f, moveLocation.y - 0.95f, moveLocation.z - 3.25f);
                        break;
                    default:
                        //In case of extending outside of the given Rooms
                        Debug.Log("No Room under that number existant");
                        break;
                }
            }
        }

        if (isNightlight)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Checks if there is a Nightlight active in the Outlet
                if (nightLight.activeSelf)
                {
                    nightLight.SetActive(false);
                    hasNightlight = true;
                    Debug.Log("Picked up Light");
                }
                else
                {   
                    //Checks if player has Nightlight in possession
                    if (hasNightlight)
                    {
                        nightLight.SetActive(true);
                        hasNightlight = false;
                        Debug.Log("Used up Light");
                    }
                    else
                    {
                        Debug.Log("No Nightlight in possession");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter (Collider collision)
    {
        //Collision detection on Doors
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = true;
            isDoor = true;
            var DoorTo = collision.gameObject.GetComponent<Door>();
            moveLocation = DoorTo.ConnectedPosition;
            RoomNum = (int)DoorTo.toRoom;
        }

        if (collision.gameObject.tag == "Outlet")
        {
            nightLight = collision.transform.GetChild(2).gameObject;
            canInteract = true;
            isNightlight = true;
            Debug.Log(nightLight);
        }
    }

    private void OnTriggerStay (Collider collision)
    {
        //Be able to constantly use Doors when going through once (would count as leaving when not checking it on in this field)
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = true;
            isDoor = true;
        }

        if (collision.gameObject.tag == "Outlet")
        {
            canInteract = true;
            isNightlight = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        //Disable any interactions with Doors while out of bounds
        if (collision.gameObject.tag == "Doorway")
        {
            canInteract = false;
            isDoor = false;
        }

        //Disable any interactions with nightlights while out of bounds
        if (collision.gameObject.tag == "Outlet")
        {
            canInteract = false;
            isNightlight = false;
        }
    }
}
