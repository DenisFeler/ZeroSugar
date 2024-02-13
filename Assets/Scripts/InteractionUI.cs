using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public GameObject InteractionButtonUI;
    public GameObject InteractionButtonBuildUpUI;
    public GameObject InteractionButtonGoneUI;
    public bool canShowUI = false;
    public bool playedOnce = false;

    void FixedUpdate()
    {
        if (canShowUI)
        {
            InteractionButtonUI.SetActive(true);
            playedOnce = true;
        }
        else
        {
            if (playedOnce)
            {
                StartCoroutine(InteractionDissappearance());
            }
        }
    }

    IEnumerator InteractionDissappearance()
    {
        InteractionButtonUI.SetActive(false);
        InteractionButtonBuildUpUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        InteractionButtonBuildUpUI.SetActive(false);
        InteractionButtonGoneUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        InteractionButtonGoneUI.SetActive(false);
        playedOnce = false;
    }
}
