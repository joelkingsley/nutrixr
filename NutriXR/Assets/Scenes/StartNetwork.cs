using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using Oculus.Avatar2;
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
        NetworkManagerWithActions.singleton.OnStartServerAction += StartAdvertise;
        #if UNITY_EDITOR
        //This app runs in the Unity Editor
        //NetworkManager.singleton.StartServer();
        NetworkManager.singleton.StartHost();

        Debug.Log("Is Editor. Start Hoste, Advertise");
        #endif
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void StartAdvertise()
    {
        Debug.LogError("I am in pain");
        networkDiscovery.AdvertiseServer();
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

        if (!NetworkManagerWithActions.singleton.isNetworkActive)
        {
            scanButton.GetComponentInChildren<TextMeshProUGUI>().text = "Scan";
            scanButton.GetComponent<Button>().interactable = true;
        }
    }

    public void StartServer()
    {
        NetworkManager.singleton.StartHost();
    }

    public void ConnectServer()
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
