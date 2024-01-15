using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPos : MonoBehaviour
{
    //private GameMaster gm;
    private GameObject player;
    private PlayerController pc;

    public enum NightLight
    {
        ChildBedroomNL,
        UpperHallwayNL,
        LowerHallwayNL,
        KitchenNL,
        LivingroomNL,
        StretchedHallwayNL
    }

    public NightLight nightLight;

    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.gameObject.GetComponent<PlayerController>();

        //gm.nightLight = pc.nightLight;
    }
}
