using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPit : MonoBehaviour
{
    //Enemy Variables
    private CapsuleCollider enemyCollider;
    private FadeUI uiFader;
    private Animator animator;

    //Player ref Variables
    private GameObject player;
    private Rigidbody playerRB;
    private PlayerController pc;

    //Interaction Variables
    [SerializeField] private bool inLight = false;
    private float killCounter = 0;

    void Start()
    {
        //Get Enemy Components
        enemyCollider = GetComponent<CapsuleCollider>();
        uiFader = GetComponent<FadeUI>();
        animator = GetComponent<Animator>();

        //Get Player reference
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.gameObject.GetComponent<Rigidbody>();
        pc = player.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {    
        if (inLight)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    IEnumerator ShadowPitKill()
    {
        playerRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        uiFader.FaderBG();
        yield return new WaitForSeconds(2f);
        uiFader.FaderTXT();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            killCounter = killCounter + 1 * Time.deltaTime;
        }

        if (collision.gameObject.tag == "NightLight")
        {
            inLight = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            killCounter = killCounter + 1 * Time.deltaTime;

            if (killCounter >= 1.5f)
            {

                StartCoroutine(ShadowPitKill());
            }
        }

        if (collision.gameObject.tag == "NightLight")
        {
            inLight = true;
        }

        if (collision.gameObject.tag != "NightLight" && collision.gameObject.tag != null)
        {
            inLight = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            killCounter = 0;
        }

        if (collision.gameObject.tag == null || collision.gameObject.tag == "NightLight")
        {
            inLight = false;
        }
    }
}
