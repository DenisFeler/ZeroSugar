using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticShadow : MonoBehaviour
{
    //Enemy Variables
    private SphereCollider enemyCollider;
    private Animator animator;

    //Sight Variable
    private bool hasLineOfSight = false;
    public float ExplosionDistance = 5;
    public float InLightDuration = 50;

    //Player ref Variables
    private GameObject player;
    private PlayerController pc;

    //Flashlight Variables
    private GameObject flashLight;
    private FlashlightController flc;
    private bool inLight = false;
    [SerializeField] private int inLightCounter = 0;

    private void Start()
    {
        //Get Enemy Variables
        enemyCollider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();

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

            if (hasLineOfSight)
            {
                if (!pc.facingRight && currentDistance <= ExplosionDistance && flc.turnedOn) //Burn away when Player looks at it in range with flashlight turned on
                {
                    inLight = true;
                    if (inLight)
                    {
                        if (inLightCounter >= InLightDuration)
                        {
                            StartCoroutine(Exploding());
                        }
                        else
                        {
                            inLightCounter++;
                        }
                    }
                    else
                    {
                        inLightCounter = 0;
                    }
                }
                else if (!pc.facingRight && currentDistance > ExplosionDistance && flc.turnedOn && !inLight) //Reset burn away counter
                {
                    inLightCounter = 0;
                }
                else if (pc.facingRight) //Reset burn away counter when turning around
                {
                    inLightCounter = 0;
                    inLight = false;
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
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
                if (pc.facingRight && currentDistance <= ExplosionDistance && flc.turnedOn) //Burn away when Player looks at it in range with flashlight turned on
                {
                    inLight = true;
                    if (inLight)
                    {
                        if (inLightCounter >= InLightDuration)
                        {
                            StartCoroutine(Exploding());
                        }
                        else
                        {
                            inLightCounter++;
                        }
                    }
                    else
                    {
                        inLightCounter = 0;
                    }
                }
                else if (pc.facingRight && currentDistance > ExplosionDistance && flc.turnedOn && !inLight) //Reset burn away counter
                {
                    inLightCounter = 0;
                }
                else if (!pc.facingRight) //Reset burn away counter when turning around
                {
                    inLightCounter = 0;
                    inLight = false;
                }
                else //Boolean reset to be out of light
                {
                    inLight = false;
                }
            }
        }
    }

    IEnumerator Exploding()
    {
        animator.SetBool("IsExploding", true);
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!pc.facingRight && !flc.turnedOn && !inLight) //Kill player when colliding and having flashlight off | Right Side
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (pc.facingRight && !flc.turnedOn && !inLight) //Kill player when colliding and having flashlight off | Left Side
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
