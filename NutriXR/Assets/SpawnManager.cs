using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public GameObject[] SpawnPositions;
    public GameObject CameraRig;
    [SyncVar] public int numPlayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        CmdUpdateNumPlayers();

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        CameraRig.transform.position = SpawnPositions[numPlayers-1 % SpawnPositions.Length].transform.position;
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateNumPlayers()
    {
        numPlayers = NetworkManagerWithActions.singleton.numPlayers;
    }




    // Update is called once per frame
    void Update()
    {

    }

}
