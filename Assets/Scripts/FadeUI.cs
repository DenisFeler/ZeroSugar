using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadingCanvasBackground;
    [SerializeField] private CanvasGroup fadingCanvasText;

    public void FaderBG()
    {
        fadingCanvasBackground.DOFade(1, 1);
    }

    public void FaderTXT()
    {
        fadingCanvasText.DOFade(1, 2);
    }
}
