using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogBarker : MonoBehaviour
{
    private bool barking = false;
    public AudioSource[] dogSounds;
    [SerializeField] private Animator barker;

    void FixedUpdate()
    {
        StartCoroutine(Barker());
    }

    IEnumerator Barker()
    {
        if (!barking)
        {
            barking = true;
            barker.SetBool("IsBarking", true);
            dogSounds[Random.Range(0, 2)].Play();
            yield return new WaitForSeconds(0.5f);
            barker.SetBool("IsBarking", false);
            dogSounds[Random.Range(0, 2)].Stop();
            yield return new WaitForSeconds(10f);
            barking = false;
        }
    }
}
