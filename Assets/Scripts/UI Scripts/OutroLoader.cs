using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroLoader : MonoBehaviour
{
    public float changeTimer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    
    private void FixedUpdate()
    {
        changeTimer -= Time.deltaTime;
        if(changeTimer <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
