using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject ConnectedTo;
    public Vector3 ConnectedPosition;

    public enum Rooms
    {
        UpperHallway2ChildBedroom, //0
        ChildBedroom2UpperHallway, //1
        UpperHallway2LowerHallway, //2
        LowerHallway2UpperHallway, //3
        LowerHallway2Livingroom, //4
        Livingroom2LowerHallway, //5
        LowerHallway2Kitchen, //6
        Kitchen2LowerHallway, //7
        Kitchen2Livingroom, //8
        Livingroom2Kitchen, //9
        Kitchen2Cellar, //10
        Cellar2Kitchen, //11
        LowerHallway2StretchedHallway, //12
        StretchedHallway2LowerHallway, //13
        StretchedHallway2Office, //14
        Office2StretchedHallway //15
    }

    public Rooms toRoom = 0;
    
    private void Awake()
    {
        ConnectedPosition = new Vector3(ConnectedTo.transform.localPosition.x, ConnectedTo.transform.localPosition.y, ConnectedTo.transform.localPosition.z);
    }
}
