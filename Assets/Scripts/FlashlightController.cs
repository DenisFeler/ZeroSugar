using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    //Player Variables
    private GameObject player;
    private PlayerController pc;

    //Flashlight Variables
    public Light flashLight;
    public bool turnedOn = true;

    //Flash flickering on low battery
    private bool isFlickering = false;
    private float timeDelay;

    //Battery Variables
    private double batteryMaxCapacity = 90;
    private double batteryCurrentCapacity;
    public double decayRate;
    public double chargeRate;
    bool charging = false;
    [SerializeField] private GameObject batteryCharge1;
    [SerializeField] private GameObject batteryCharge2;
    [SerializeField] private GameObject batteryCharge3;

    //Get References to player Script and pre set the battery
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.gameObject.GetComponent<PlayerController>();

        batteryCurrentCapacity = batteryMaxCapacity;
    }

    private void Update()
    {
        //Turning Flashlight off interaction
        if (Input.GetMouseButtonDown(0))
        {
            turnedOn = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            turnedOn = true;
        }

        ChargeFlashlight();
    }

    private void FixedUpdate()
    {
        FlashLight();
        
        if (charging)
        {
            batteryCurrentCapacity += 1 * chargeRate;

            if (batteryCurrentCapacity >= 60)
            {
                //Battery Charge Display
                batteryCharge1.SetActive(true);
                batteryCharge2.SetActive(true);
                batteryCharge3.SetActive(true);
            }
            else if (batteryCurrentCapacity >= 30)
            {
                //Battery Charge Display
                batteryCharge1.SetActive(false);
                batteryCharge2.SetActive(true);
                batteryCharge3.SetActive(true);
            }
            else if (batteryCurrentCapacity >= 0)
            {
                //Battery Charge Display
                batteryCharge1.SetActive(false);
                batteryCharge2.SetActive(false);
                batteryCharge3.SetActive(true);
            }
        }
    }

    //Flashlight decay with battery managing
    void FlashLight()
    {
        if (turnedOn && batteryCurrentCapacity >= 60)
        {
            flashLight.enabled = true;
            //Battery Decay Rate
            batteryCurrentCapacity -= 1 * decayRate;
            //Battery Charge Display
            batteryCharge1.SetActive(true);
            batteryCharge2.SetActive(true);
            batteryCharge3.SetActive(true);
        }
        else if (turnedOn && batteryCurrentCapacity >= 30)
        {
            flashLight.enabled = true;
            //Battery Decay Rate
            batteryCurrentCapacity -= 1 * decayRate;
            //Battery Charge Display
            batteryCharge1.SetActive(false);
            batteryCharge2.SetActive(true);
            batteryCharge3.SetActive(true);
        }
        else if (turnedOn && batteryCurrentCapacity >= 0)
        {
            flashLight.enabled = true;
            //Battery Decay Rate
            batteryCurrentCapacity -= 1 * decayRate;
            //Battery Charge Display
            batteryCharge1.SetActive(false);
            batteryCharge2.SetActive(false);
            batteryCharge3.SetActive(true);
            //Flashlight flickering
            if (isFlickering == false)
            {
                StartCoroutine(FlickeringLight());
            }
        }
        else if (turnedOn && batteryCurrentCapacity <= 0)
        {
            flashLight.enabled = false;
            //Battery Charge Display
            batteryCharge1.SetActive(false);
            batteryCharge2.SetActive(false);
            batteryCharge3.SetActive(false);
            turnedOn = false;
        }
        else if (!turnedOn)
        {
            flashLight.enabled = false;
        }
    }

    void ChargeFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.R) || pc.hidden)
        {
            flashLight.enabled = false;
            turnedOn = false;
            charging = true;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            flashLight.enabled = true;
            turnedOn = true;
            charging = false;
            if (batteryCurrentCapacity >= 90)
            {
                batteryCurrentCapacity = batteryMaxCapacity;
            }
        }
    }

    //Flashlight Flicker
    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        flashLight.enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        flashLight.enabled = true;
        timeDelay = Random.Range(0.01f, 0.1f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
