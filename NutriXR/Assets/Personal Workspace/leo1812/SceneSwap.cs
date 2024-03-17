using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == "TestScene")
            {

                SceneManager.LoadScene("Personal Workspace/leo1812/SecondScene");
            }
        }
    }
}
