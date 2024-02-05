using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public GameObject InteractionButtonUI;
    public bool canShowUI = false;

    void FixedUpdate()
    {
        if (canShowUI)
        {
            InteractionButtonUI.SetActive(true);
        }
        else
        {
            InteractionButtonUI.SetActive(false);
        }
    }
}
