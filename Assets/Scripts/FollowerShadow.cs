using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerShadow : MonoBehaviour
{
    //Collision Variables
    private Rigidbody rb;
    private CapsuleCollider enemyCollider;

    //Movement Variables
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    private bool hasLineOfSight = false;
    private bool startChase = false;
    private Vector3 startPosition;
    [SerializeField] private bool WanderBack = false;

    //Player ref Variables
    private GameObject player;
    private PlayerController pc;
    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;
    [SerializeField] private bool inLight = false;

    private void Start()
    {
        //Get Enemy Collisions
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();

        //Get spawn position
        startPosition = transform.position;

        //Get Player ref
        player = GameObject.FindGameObjectWithTag("Player");
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
                }
                else if (!pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    currentMoveSpeed = minMoveSpeed;
                    startChase = false;
                    inLight = true;
                }
                else if (!pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
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
                if (currentMoveSpeed >= maxSpeed && startChase)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else if (currentMoveSpeed <= maxSpeed && startChase)
                {

                    currentMoveSpeed += acceleration;
                }

                if (pc.facingRight && !inLight) //Charge Player when not looking in direction & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    currentMoveSpeed = minMoveSpeed;
                    startChase = false;
                    inLight = true;
                }
                else if (!pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
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
