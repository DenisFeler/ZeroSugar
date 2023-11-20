using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject ConnectedTo;
    public Vector3 ConnectedPosition;

    private void Awake()
    {
        ConnectedPosition = new Vector3(ConnectedTo.transform.localPosition.x, ConnectedTo.transform.localPosition.y, ConnectedTo.transform.localPosition.z);
    }
}
