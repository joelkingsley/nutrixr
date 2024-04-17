using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour

{
    private GameObject ovrCameraRig;

    private void Start()
    {
        ovrCameraRig = GameObject.Find("OVRCameraRig");
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        transform.position = ovrCameraRig.transform.position;
    }
}
