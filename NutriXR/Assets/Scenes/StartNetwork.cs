using Mirror;
using UnityEngine;
public class StartNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        #if UNITY_EDITOR
        GetComponent<NetworkManager>().StartServer();
        #else
        //GetComponent<NetworkManager>().StartClient();
        GetComponent<NetworkManager>().StartHost();
        #endif
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
