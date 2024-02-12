using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChargerShadow : MonoBehaviour
{
    //Enemy Variables
    private Rigidbody rb;
    private CapsuleCollider enemyCollider;
    private FadeUI uiFader;

    //Movement Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowRatio;
    private bool hasLineOfSight = false;
    private bool hadLineOfSight = false;

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
        //Get Enemy Collisions
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();

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

            if (hasLineOfSight)
            {
                if (pc.facingRight && !inLight) //Charge Player when not looking in direction & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else if (!pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed / slowRatio) * Time.deltaTime);
                    inLight = true;
                }
                else if (!pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else if (!pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
                    hadLineOfSight = true;
                }
            }
            else if (!hasLineOfSight && hadLineOfSight)//Wander back to spawn point when losing sight
            {
                Debug.Log("Avoided");
                Destroy(gameObject);
            }
            else
            {

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
                if (!pc.facingRight && !inLight) //Charge Player when not looking in direction & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else if (pc.facingRight && currentDistance <= 9 && flc.turnedOn) //Get away from player when looking in direction & being too close while flashlight is on
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed / slowRatio) * Time.deltaTime);
                    inLight = true;
                }
                else if (pc.facingRight && currentDistance > 9 && flc.turnedOn && !inLight) //Charge Player when looking in direction & being far away from flashlight & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else if (pc.facingRight && !flc.turnedOn && !inLight) //Charge Player when looking in direction & flashlight not on & not being in any light
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
                    hadLineOfSight = true;
                }
            }
            else if (!hasLineOfSight && hadLineOfSight)//Wander back to spawn point when losing sight
            {
                Debug.Log("Avoided");
                Destroy(gameObject);
            }
            else
            {

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
