using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class FadeUI : MonoBehaviour
{
    //Death Screen Variables
    [SerializeField] private GameObject CanvasBackground;
    [SerializeField] private CanvasGroup fadingCanvasText;

    //Camera Variables
    private GameObject physCamera;

    private void Start()
    {
        //Get Camera Components on game startup
        physCamera = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
    }

    public void FaderBG()
    {
        CanvasBackground.transform.DOMoveY(1080/2, 0.5f, true);
        physCamera.transform.DORotate(new Vector3(60, 0, 0), 2, default);
    }

    public void FaderTXT()
    {
        fadingCanvasText.DOFade(1, 2);
    }
}
