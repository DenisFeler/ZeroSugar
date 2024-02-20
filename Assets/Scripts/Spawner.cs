using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyToSpawn;
    private Rigidbody rb;
    [SerializeField] private Vector3 SpawnTo;

    private void Start()
    {
        rb = EnemyToSpawn.gameObject.GetComponent<Rigidbody>();
        SpawnTo = EnemyToSpawn.transform.localPosition;
    }

    private void OnTriggerEnter(Collider collision)
    {
        EnemyToSpawn.transform.localPosition = new Vector3(SpawnTo.x, SpawnTo.y, SpawnTo.z - 100);
        rb.useGravity = true;
    }
}
