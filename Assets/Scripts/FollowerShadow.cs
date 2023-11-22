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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();

        //Get Player ref
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.gameObject.GetComponent<PlayerController>();
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
                if (moveSpeed >= maxSpeed && startChase)
                {
                    currentMoveSpeed = maxSpeed;
                }
                else
                {
                    currentMoveSpeed += acceleration;
                }

                if (pc.facingRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                    startChase = true;
                }
                else if (!pc.facingRight && currentDistance <= 9)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -moveSpeed * Time.deltaTime);
                    currentMoveSpeed = moveSpeed;
                    startChase = false;
                }
                else if (!pc.facingRight && currentDistance > 9)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
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
                if (pc.facingRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -moveSpeed * Time.deltaTime);
                }
                else if (!pc.facingRight && currentDistance <= 9)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else if (!pc.facingRight && currentDistance > 9)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -moveSpeed * Time.deltaTime);
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
