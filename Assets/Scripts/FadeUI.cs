using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private GameObject CanvasBackground;
    [SerializeField] private CanvasGroup fadingCanvasText;

    public void FaderBG()
    {
        CanvasBackground.transform.DOMoveY(1080/2, 2, true);
    }

    public void FaderTXT()
    {
        fadingCanvasText.DOFade(1, 2);
    }
}
