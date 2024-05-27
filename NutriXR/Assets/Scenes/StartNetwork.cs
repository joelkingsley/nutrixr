using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartNetwork : MonoBehaviour
{
    //readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;
    private ServerResponse serverResponse;

    [SerializeField] private GameObject scanButton;
    [SerializeField] private GameObject joinButton;

    // Start is called before the first frame update
    private void Start()
    {
        networkDiscovery = GetComponent<NetworkDiscovery>();

        #if UNITY_EDITOR
        //This app runs in the Unity Editor
        //GetComponent<NetworkManager>().StartServer();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        Debug.Log("Is Editor. Start Hoste, Advertise");
        #endif
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    // Update is called once per frame
    private void Update()
    {

    }

    public void StartDiscovery()
    {
        //Start scanning
        networkDiscovery.StartDiscovery();
        scanButton.GetComponentInChildren<TextMeshProUGUI>().text = "Scanning...";
        scanButton.GetComponent<Button>().interactable = false;

        //Stop scanning after 10 seconds
        StartCoroutine(StopDiscovery(10.0f));
    }

    private IEnumerator StopDiscovery(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        networkDiscovery.StopDiscovery();
        scanButton.GetComponentInChildren<TextMeshProUGUI>().text = "Scan";
        scanButton.GetComponent<Button>().interactable = true;
    }

    public void StartSingleplayer()
    {
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void StartMultiplayer()
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(serverResponse.uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        Debug.Log("Server Discovered");
        //Server was found
        serverResponse = info;
        networkDiscovery.StopDiscovery();
        scanButton.SetActive(false);
        joinButton.SetActive(true);
        joinButton.GetComponentInChildren<TextMeshProUGUI>().text = info.uri.ToString();
    }
}
