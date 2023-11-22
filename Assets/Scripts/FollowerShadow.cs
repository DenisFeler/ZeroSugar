using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerShadow : MonoBehaviour
{
    //Collision Variables
    private Rigidbody rb;
    private CapsuleCollider enemyCollider;

    //Movement Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    private bool hasLineOfSight = false;
    private bool startChase = false;

    //Player ref Variables
    private GameObject player;
    private PlayerController pc;
    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();

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
                if (moveSpeed >= maxSpeed && !startChase)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else
                {
                    currentMoveSpeed += acceleration;
                }

                if (pc.facingRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && currentDistance <= 9 && flc.turnedOn)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    currentMoveSpeed = moveSpeed;
                    startChase = false;
                }
                else if (!pc.facingRight && currentDistance > 9 && flc.turnedOn)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && !flc.turnedOn)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
            }
        }

        //Raycast to the Right to search for player
        RaycastHit hitL;        
        if (Physics.Raycast(transform.position, -Vector3.right * 100, out hitL))
        {
            hasLineOfSight = hitL.collider.CompareTag("Player");
            float currentDistance = Vector3.Distance(transform.position, player.transform.position);

            if (hasLineOfSight)
            {
                if (moveSpeed >= maxSpeed && !startChase)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else
                {
                    currentMoveSpeed += acceleration;
                }

                if (!pc.facingRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (pc.facingRight && currentDistance <= 9 && flc.turnedOn)
                {
                    if (currentDistance <= 9)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime);
                        currentMoveSpeed = moveSpeed;
                        startChase = false;
                    }
                    else if (currentDistance > 9)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                        startChase = true;
                    }
                }
                else if (pc.facingRight && !flc.turnedOn)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -currentMoveSpeed * Time.deltaTime);
                    startChase = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
