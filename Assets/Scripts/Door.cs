using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject ConnectedTo;
    public Vector3 ConnectedPosition;

    public enum Rooms
    {
        UpperHW2ChildBR, //0
        ChildBR2UpperHW, //1
        UpperHW2LowerHW, //2
        LowerHW2UpperHW, //3
        LowerHW2Livingroom, //4
        Livingroom2LowerHW, //5
        LowerHW2Kitchen, //6
        Kitchen2LowerHW, //7
        Kitchen2Livingroom, //8
        Livingroom2Kitchen, //9
        Kitchen2Cellar, //10
        Cellar2Kitchen, //11
        LowerHW2StretchedHW, //12
        StretchedHW2LowerHW, //13
        StretchedHW2Office, //14
        Office2StretchedHW //15
    }

    public Rooms toRoom = 0;

    private void Awake()
    {
        ConnectedPosition = new Vector3(ConnectedTo.transform.localPosition.x, ConnectedTo.transform.localPosition.y, ConnectedTo.transform.localPosition.z);
    }
}
