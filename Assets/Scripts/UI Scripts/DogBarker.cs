using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogBarker : MonoBehaviour
{
    private bool barking = false;
    public Image firstBark;
    public Image secondBark;
    public Image thirdBark;

    void FixedUpdate()
    {
        StartCoroutine(Barker());
    }

    IEnumerator Barker()
    {
        if (!barking)
        {
            barking = true;
            thirdBark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            thirdBark.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            secondBark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            secondBark.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            firstBark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            firstBark.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            secondBark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            secondBark.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            thirdBark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            thirdBark.gameObject.SetActive(false);
            yield return new WaitForSeconds(10f);
            barking = false;
        }
    }
}
