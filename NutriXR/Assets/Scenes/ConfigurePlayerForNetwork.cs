using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Oculus.Avatar2;
using UnityEngine;

public class ConfigurePlayerForNetwork : NetworkBehaviour
{
    [Header("Local Avatar")] [SerializeField]
    private SampleAvatarEntity localAvatar;

    [Space] [Header("Remote Avatar")] [SerializeField]
    private SampleAvatarEntity remoteAvatar;

    [Header("Remote Avatar")] [SerializeField]
    private GameObject carts;


    private void Start()
    {

    }

    private void Update()
    {

    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        localAvatar.gameObject.SetActive(false);
        remoteAvatar.gameObject.SetActive(true);

        //Spawn Carts and give authority
        NetworkServer.Spawn(Instantiate(carts, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity), connectionToClient);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            //Player is the local player

            //SetUp:
            localAvatar.gameObject.SetActive(true);
            remoteAvatar.gameObject.SetActive(false);
            localAvatar.SetBodyTracking(FindObjectOfType<SampleInputManager>());

            //Move to OVRCameraRig:
            transform.SetParent(GameObject.Find("OVRCameraRig").transform);

            //Start Streaming only after skeleton was fully loaded:
            localAvatar.OnSkeletonLoadedEvent.AddListener(StreamStarter);

        }
        else
        {
            //Player is Network player
            localAvatar.gameObject.SetActive(false);
            remoteAvatar.gameObject.SetActive(true);
        }
    }

    [Command]   //Invoked by Client, executed on Server
    public void CmdSendAvatarToServer(byte[] data)
    {
        remoteAvatar.ApplyStreamData(data);
        //Forward to all the clients:
        RpcSendAvatarToClient(data);
    }

    [ClientRpc] //Invoked by Server, executed on Client
    public void RpcSendAvatarToClient(byte[] data)
    {
        if (!isLocalPlayer)
        {
            //That means the avatar needs to be synced
            remoteAvatar.ApplyStreamData(data);
        }
    }

    //Initiate Streaming Loop
    private void StreamStarter(OvrAvatarEntity ovrAvatarEntity)
    {
        ovrAvatarEntity.StartCoroutine(StreamAvatarData());
    }

    //Steaming Loop
    private IEnumerator StreamAvatarData()
    {
        while (true)
        {
            if (isLocalPlayer)
            {
                byte[] data = localAvatar.RecordStreamData(localAvatar.activeStreamLod);
                CmdSendAvatarToServer(data);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
