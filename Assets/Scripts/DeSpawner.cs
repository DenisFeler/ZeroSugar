using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyToDespawn;

    private void OnTriggerEnter(Collider collision)
    {
        if (EnemyToDespawn != null)
        {
            Destroy(EnemyToDespawn);
        }
        else
        {
            Debug.Log("No Enemy Selected");
        }
    }
}
