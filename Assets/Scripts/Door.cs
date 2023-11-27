using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject ConnectedTo;
    public Vector3 ConnectedPosition;

    public enum Rooms
    {
        Floor2ChildBR, //0
        ChildBR2Floor, //1
        Floor2LivingRoom, //2
        LivingRoom2Floor, //3
    }

    public Rooms toRoom = 0;

    private void Awake()
    {
        ConnectedPosition = new Vector3(ConnectedTo.transform.localPosition.x, ConnectedTo.transform.localPosition.y, ConnectedTo.transform.localPosition.z);
    }
}
