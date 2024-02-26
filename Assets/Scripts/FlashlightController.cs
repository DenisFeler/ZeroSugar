using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    //Player Variables
    private GameObject player;
    private PlayerController pc;

    //Audio Variables
    public AudioSource FlashlightStaticSound;
    public AudioSource FlashlightRechargeSound;
    private bool hasPlayedSound = false;
    private float counter = 0;

    //Flashlight Variables
    public Light flashLight;
    public bool turnedOn = true;
    [SerializeField] private Animator animator;

    //Flash flickering on low battery
    private bool isFlickering = false;
    private float timeDelay;

    //Battery Variables
    private double batteryMaxCapacity = 90;
    private double batteryCurrentCapacity;
    public double decayRate;
    public double chargeRate;
    public float TimeTillChargeActive;
    private float ChargeDelay;
    bool charging = false;
    [SerializeField] private GameObject batteryCharge1;
    [SerializeField] private GameObject batteryCharge2;
    [SerializeField] private GameObject batteryCharge3;
    [SerializeField] private Image Charge1;
    [SerializeField] private Image Charge2;
    [SerializeField] private Image Charge3;

    [SerializeField] private Slider ChargeDelayTimer;

    //Get References to player Script and pre set the battery
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.gameObject.GetComponent<PlayerController>();

        ChargeDelay = TimeTillChargeActive;

        batteryCurrentCapacity = batteryMaxCapacity;
    }

    private void Update()
    {
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
                Charge1.gameObject.SetActive(true);
                Charge2.gameObject.SetActive(true);
                Charge3.gameObject.SetActive(true);
                animator.SetBool("NeedsCharging", false);
            }
            else if (batteryCurrentCapacity >= 30)
            {
                //Battery Charge Display
                batteryCharge1.SetActive(false);
                batteryCharge2.SetActive(true);
                batteryCharge3.SetActive(true);
                Charge1.gameObject.SetActive(false);
                Charge2.gameObject.SetActive(true);
                Charge3.gameObject.SetActive(true);
                animator.SetBool("NeedsCharging", true);
            }
            else if (batteryCurrentCapacity >= 0)
            {
                //Battery Charge Display
                batteryCharge1.SetActive(false);
                batteryCharge2.SetActive(false);
                batteryCharge3.SetActive(true);
                Charge1.gameObject.SetActive(false);
                Charge2.gameObject.SetActive(false);
                Charge3.gameObject.SetActive(true);
                animator.SetBool("NeedsCharging", true);
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
            Charge1.gameObject.SetActive(true);
            Charge2.gameObject.SetActive(true);
            Charge3.gameObject.SetActive(true);
            animator.SetBool("NeedsCharging", false);
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
            Charge1.gameObject.SetActive(false);
            Charge2.gameObject.SetActive(true);
            Charge3.gameObject.SetActive(true);
            animator.SetBool("NeedsCharging", true);
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
            Charge1.gameObject.SetActive(false);
            Charge2.gameObject.SetActive(false);
            Charge3.gameObject.SetActive(true);
            animator.SetBool("NeedsCharging", true);
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
            Charge1.gameObject.SetActive(false);
            Charge2.gameObject.SetActive(false);
            Charge3.gameObject.SetActive(false);
            animator.SetBool("NeedsCharging", true);
            turnedOn = false;
        }
        else if (!turnedOn)
        {
            flashLight.enabled = false;
        }

        if (turnedOn)
        {
            FlashlightStaticSound.PlayOneShot(FlashlightStaticSound.clip, 0.125f);
        }
        else
        {
            FlashlightStaticSound.Stop();
        }
    }

    void ChargeFlashlight()
    {
        if (Input.GetKey(KeyCode.R))
        {
            TimeTillChargeActive = TimeTillChargeActive - 1 * Time.deltaTime;
            ChargeLightTimer();

            if (TimeTillChargeActive <= 0)
            {
                flashLight.enabled = false;
                turnedOn = false;
                charging = true;

                if (!hasPlayedSound)
                {
                    hasPlayedSound = true;

                    FlashlightRechargeSound.PlayOneShot(FlashlightRechargeSound.clip, 0.2f);
                }
                else
                {
                    counter = counter + 1 * Time.deltaTime;

                    if (counter == FlashlightRechargeSound.clip.length)
                    {
                        hasPlayedSound = false;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            TimeTillChargeActive = ChargeDelay;
            ChargeLightTimer();
            counter = 0;
            hasPlayedSound = false;

            flashLight.enabled = true;
            turnedOn = true;
            charging = false;
            if (batteryCurrentCapacity >= 90)
            {
                batteryCurrentCapacity = batteryMaxCapacity;
            }

            FlashlightRechargeSound.Stop();
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

    public void ChargeLightTimer()
    {
        ChargeDelayTimer.value = TimeTillChargeActive;
    }
}
