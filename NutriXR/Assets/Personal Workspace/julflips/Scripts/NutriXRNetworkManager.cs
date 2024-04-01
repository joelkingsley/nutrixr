using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Mirror;

public class NutriXRNetworkManager : NetworkManager
{

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client number " + numPlayers + " connected");
    }
}
