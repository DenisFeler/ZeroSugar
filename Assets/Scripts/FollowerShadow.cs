using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowerShadow : MonoBehaviour
{
    //Enemy Variables
    private Rigidbody rb;
    private CapsuleCollider enemyCollider;
    private FadeUI uiFader;
    private Animator animator;

    //Movement Variables
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    private bool hasLineOfSight = false;
    private bool startChase = false;
    private Vector3 startPosition;
    private bool WanderBack = false;

    //Player ref Variables
    private GameObject player;
    private Rigidbody playerRB;
    private PlayerController pc;

    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;
    private bool inLight = false;

    private void Start()
    {
        //Get Enemy Components
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();
        uiFader = GetComponent<FadeUI>();
        animator = GetComponent<Animator>();

        //Get spawn position
        startPosition = transform.position;

        //Get Player ref
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.gameObject.GetComponent<Rigidbody>();
        pc = player.gameObject.GetComponent<PlayerController>();

        //Get Flashlight Components on game startup
        flashLight = GameObject.FindGameObjectWithTag("Flashlight");
        flc = flashLight.gameObject.GetComponent<FlashlightController>();
    }

    private void FixedUpdate()
    {        
        //Raycast to the Right to search for player
        RaycastHit hitR;
        if (Physics.Raycast(transform.position, Vector3.right * 100, out hitR))
        {
            hasLineOfSight = hitR.collider.CompareTag("Player");
            float currentDistance = Vector3.Distance(transform.position, player.transform.position);

            if(hasLineOfSight)
            {
                WanderBack = false;

                if (currentMoveSpeed >= maxSpeed && startChase) //Stay at maxSpeed when chasing (aka Limiter)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else if (currentMoveSpeed <= maxSpeed && startChase) //Accelerate while chasing
                {
                    
                    currentMoveSpeed += acceleration;
                }

                if (pc.facingRight && !inLight) //Charge Player when not looking in direction & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else if (!pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    currentMoveSpeed = minMoveSpeed;
                    startChase = false;
                    inLight = true;

                    animator.SetBool("IsWalking", false);
                }
                else if (!pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else if (!pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;

                    animator.SetBool("IsWalking", false);
                }
            }
            else //Wander back to spawn point when losing sight
            {
                WanderBack = true;
                if (WanderBack)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPosition, minMoveSpeed * Time.deltaTime);
                }
            }
        }

        //Raycast to the Left to search for player
        RaycastHit hitL;        
        if (Physics.Raycast(transform.position, -Vector3.right * 100, out hitL))
        {
            hasLineOfSight = hitL.collider.CompareTag("Player");
            float currentDistance = Vector3.Distance(transform.position, player.transform.position);

            if (hasLineOfSight)
            {
                WanderBack = false;

                if (currentMoveSpeed >= maxSpeed && startChase) //Stay at maxSpeed when chasing (aka Limiter)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else if (currentMoveSpeed <= maxSpeed && startChase) //Accelerate while chasing
                {

                    currentMoveSpeed += acceleration;
                }

                if (!pc.facingRight && !inLight) //Charge Player when not looking in direction & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else if (pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    currentMoveSpeed = minMoveSpeed;
                    startChase = false;
                    inLight = true;

                    animator.SetBool("IsWalking", false);
                }
                else if (pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else if (pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;

                    animator.SetBool("IsWalking", true);
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;

                    animator.SetBool("IsWalking", false);
                }
            }
            else //Wander back to spawn point when losing sight
            {
                WanderBack = true;
                if (WanderBack)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPosition, minMoveSpeed * Time.deltaTime);
                }
            }
        }
    }

    IEnumerator KillingTime()
    {
        playerRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        uiFader.FaderBG();
        yield return new WaitForSeconds(1f);
        uiFader.FaderTXT();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(KillingTime());
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "NightLight")
        {
            inLight = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "NightLight")
        {
            inLight = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == null || collision.gameObject.tag == "NightLight")
        {
            inLight = false;
        }
    }
}
