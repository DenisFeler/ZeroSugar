using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Visual Variables
    public GameObject modelCont;

    //Sound Variables
    public AudioSource WalkingSound;
    public AudioSource NLInSound;
    public AudioSource NLOutSound;

    //Movement Variables
    float horizontalInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowDown;
    private float defaultMoveSpeed;
    [HideInInspector] public bool facingRight = true;
    private Vector3 movement;

    //Collision Variables
    private Rigidbody rb;
    private SphereCollider playerCollider;

    //Interaction Variable
    [HideInInspector] public bool canInteract = false;
    public GameObject InteractionUICanvas;
    private InteractionUI interactUI;

    //Door Variables
    private bool isDoor = false;
    private Vector3 moveLocation;
    private int RoomNum;

    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;

    //Nightlight Variables
    public GameObject nightLight;
    private bool isNightlight = false;
    [HideInInspector] public bool hasNightlight = false;
    private GameMaster gm;

    //Hiding Variables
    private bool canHide = false;
    [HideInInspector] public bool hidden = false;
    private Vector3 currentPosition;
    private Vector3 hideLocation;
    
    //Camera Variables
    private GameObject physCamera;
    private CinemachineVirtualCamera vCam;
    public float fieldOfViewHiding;
    public GameObject TransitionBlendUI;

    private void Start()
    {
        //Get Player Components on game startup
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
        defaultMoveSpeed = moveSpeed;

        //Get Flashlight Components on game startup
        flashLight = GameObject.FindGameObjectWithTag("Flashlight");
        flc = flashLight.gameObject.GetComponent<FlashlightController>();

        //Get Camera Components on game startup
        physCamera = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
        vCam = physCamera.gameObject.GetComponent<CinemachineVirtualCamera>();

        //Get Checkpoint Components on game startup
        //gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        interactUI = InteractionUICanvas.gameObject.GetComponent<InteractionUI>();
    }

    private void Update()
    {
        //Check for interactable objects encountered
        if (canInteract)
        {
            Interaction();
            interactUI.canShowUI = true;
        }
        else
        {
            interactUI.canShowUI = false;
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
            movement = new Vector3(horizontalInput * moveSpeed / slowDown, 0f, 0f);
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
        Vector3 currentScalePlayer = modelCont.transform.localScale;
        Vector3 currentScaleFlash = flashLight.transform.localScale;
        currentScalePlayer.z *= -1;
        currentScaleFlash.y *= -1;
        modelCont.transform.localScale = currentScalePlayer;
        flashLight.transform.localScale = currentScaleFlash;

        facingRight = !facingRight;
    }

    //Interactable World Objects
    void Interaction()
    {
        //Checks for Doors
        if (isDoor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Moving to the Door given through trigger collision
                switch (RoomNum)
                {
                    case 0: //Entering Child Bedroom from Upper Hallway
                        //Adaptable Positioning on door entering
                        //ExitfromRight
                        // gameObject.transform.localPosition = new Vector3(10000f, 10000f, moveLocation.z);
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 1: //Entering Upper Hallway from Child Bedroom
                        //Adaptable Positioning on door entering
                        //ExitfromLeft
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 2: //Entering Lower Hallway from Upper Hallway
                        //Adaptable Positioning on door entering
                        //ExitDownstairs
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1f, moveLocation.y + 1f, moveLocation.z + 4f);
                        break;
                    case 3: //Entering Upper Hallway from Lower Hallway
                        //Adaptable Positioning on door entering
                        //ExitUpstairs
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1f, moveLocation.y + 1f, moveLocation.z + 4f);
                        break;
                    case 4: //Entering Livingroom from Lower Hallway
                        //Adaptable Positioning on door entering
                        //ExitfromBack
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z - 2.85f);
                        break;
                    case 5: //Entering Lower Hallway from Livingroom
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z - 2.85f);
                        break;
                    case 6: //Entering Kitchen from Lower Hallway
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 7: //Entering Lower Hallway from Kitchen
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 8: //Entering Livingroom from Kitchen
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z - 2.85f);
                        break;
                    case 9: //Entering Kitchen from Livingroom
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z - 2.85f);
                        break;
                    case 10: //Entering Cellar from Kitchen
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 11: //Entering Kitchen from Cellar
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 12: //Entering Stretched Hallway from Lower Hallway
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x - 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 13: //Entering Lower Hallway from Stretched Hallway
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x + 1.75f, moveLocation.y - 1.45f, moveLocation.z);
                        break;
                    case 14: //Entering Office from Stretched Hallway
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z -2.85f);
                        break;
                    case 15: //Entering Stretched Hallway from Office
                        //Adaptable Positioning on door entering
                        gameObject.transform.localPosition = new Vector3(moveLocation.x, moveLocation.y - 1.45f, moveLocation.z -2.85f);
                        break;
                    default:
                        //In case of extending outside of the given Rooms
                        Debug.Log("No Room under that number existant");
                        break;
                }
            }
        }

        //Checks for Nightlights
        if (isNightlight)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Checks if there is a Nightlight active in the Outlet
                if (nightLight.activeSelf)
                {
                    nightLight.SetActive(false);
                    hasNightlight = true;
                    NLOutSound.Play();
                }
                else
                {   
                    //Checks if player has Nightlight in possession
                    if (hasNightlight)
                    {
                        nightLight.SetActive(true);
                        hasNightlight = false;
                        NLInSound.Play();
                        //gm.lastCheckpointPos = transform.position;
                        //gm.nightLight = nightLight;
                    }
                    else
                    {
                        Debug.Log("No Nightlight in possession");
                    }
                }
            }
        }

        //Checks for Hidingspots
        if (canHide)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Flip checking if the player is hidden or not
                if (!hidden)
                {
                    //Saves the player position before going into hiding
                    currentPosition = gameObject.transform.localPosition;
                    //Reposition the player into the hiding spots location
                    gameObject.transform.localPosition = new Vector3(hideLocation.x, hideLocation.y, hideLocation.z + 2f);
                    //Zero movespeed to pseudo disable movement
                    moveSpeed *= 0;
                    //Change FOV when hiding, with adaptable variable
                    vCam.m_Lens.FieldOfView = fieldOfViewHiding;
                    //Turn flashlight off, as to not empty out the whole flashlight while hiding
                    flc.turnedOn = false;
                    //Flip switch variable
                    hidden = true;
                }
                else
                {
                    //Get back into position that was saved before
                    gameObject.transform.localPosition = currentPosition;
                    //Get back into default movespeed, previously set up
                    moveSpeed = defaultMoveSpeed;
                    //Reset FOV to default
                    vCam.m_Lens.FieldOfView = 60f;
                    //Turn flashlight on
                    flc.turnedOn = true;
                    //Flip switch variable
                    hidden = false;
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
            moveLocation = DoorTo.AbsoluteWorldPosition;
            RoomNum = (int)DoorTo.toRoom;
        }
        //Collision detection on Nightlights
        if (collision.gameObject.tag == "Outlet")
        {
            nightLight = collision.transform.GetChild(2).gameObject;
            canInteract = true;
            isNightlight = true;
        }
        //Collision detection on Hidingspots
        if (collision.gameObject.tag == "HidingSpace")
        {
            canInteract = true;
            canHide = true;
            hideLocation = collision.gameObject.transform.localPosition;
        }

        //Collision detection on Shadow Pits
        if (collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        //Be able to constantly use Nightlights/Outlets
        if (collision.gameObject.tag == "Outlet")
        {
            canInteract = true;
            isNightlight = true;
        }
        //Be able to constantly use Hidingspots
        if (collision.gameObject.tag == "HidingSpace")
        {
            canInteract = true;
            canHide = true;
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
        //Disable any interactions with Hidingspots while out of bounds
        if (collision.gameObject.tag == "HidingSpace")
        {
            canInteract = false;
            canHide = false;
        }
    }
}
