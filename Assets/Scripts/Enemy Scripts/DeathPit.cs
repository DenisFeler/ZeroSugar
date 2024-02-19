using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DeathPit : MonoBehaviour
{
    //Enemy Variables
    private CapsuleCollider enemyCollider;
    private FadeUI uiFader;
    private Animator animator;
    private SpriteRenderer[] spriteRender;

    //Player ref Variables
    private GameObject player;
    private Rigidbody playerRB;
    private PlayerController pc;

    //Interaction Variables
    [SerializeField] private bool inLight = true;
    private float killCounter = 0;

    void Start()
    {
        //Get Enemy Components
        enemyCollider = GetComponent<CapsuleCollider>();
        uiFader = GetComponent<FadeUI>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponentsInChildren<SpriteRenderer>();

        //Get Player reference
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.gameObject.GetComponent<Rigidbody>();
        pc = player.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {    
        if (inLight)
        {
            foreach (SpriteRenderer render in spriteRender)
            {
                render.DOColor(new Color(1f, 1f, 1f, 0f), 1f);
            }

            //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            foreach (SpriteRenderer render in spriteRender)
            {
                render.DOColor(new Color(1f, 1f, 1f, 1f), 3f);
            }

            //gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
        if (!inLight)
        {
            if (collision.gameObject.tag == "Player")
            {
                killCounter = killCounter + 1 * Time.deltaTime;
            }
        }

        if (collision.gameObject.tag == "NightLight")
        {
            inLight = true;
        }

        if (collision.gameObject.tag == "NightLightOut")
        {
            inLight = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!inLight)
        {
            if (collision.gameObject.tag == "Player")
            {
                killCounter = killCounter + 1 * Time.deltaTime;

                if (killCounter >= 1.5f)
                {
                    StartCoroutine(ShadowPitKill());
                }
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

        if (collision.gameObject.tag == "NightLightOut" && !collision.gameObject.activeSelf)
        {
            inLight = false;
        }

        if (collision.gameObject.tag == null)
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
