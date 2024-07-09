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
        yield return new WaitForSeconds(0.2f);

        GameObject localPlayer = NetworkClient.localPlayer.gameObject;
        Transform spawnPos = SpawnPositions[numPlayers - 1 % SpawnPositions.Length].transform;

        DataLogger.Log("SpawnManager", "Spawning player at location " + (numPlayers - 1 % SpawnPositions.Length) + ". NumPlayers is " + numPlayers);

        //Teleport camera and player prefab to spawn location
        CameraRig.transform.position = spawnPos.position;
        CameraRig.transform.rotation = spawnPos.rotation;
        //localPlayer.transform.position = spawnPos;

        //Spawn ShoppingCart at new location
        localPlayer.GetComponent<ConfigurePlayerForNetwork>().SpawnShoppingCart(spawnPos.position + new Vector3(-1, 0, 0), Quaternion.Euler(0, 180, 0));
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
