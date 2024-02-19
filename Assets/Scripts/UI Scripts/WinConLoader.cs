using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class WinConLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup EndingFader;
    
    private void OnTriggerEnter(Collider collision)
    {
        StartCoroutine(EndingLoad());
    }

    IEnumerator EndingLoad()
    {
        EndingFader.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
