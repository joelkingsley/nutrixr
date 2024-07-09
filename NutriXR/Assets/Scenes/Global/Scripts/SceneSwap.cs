using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /*void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            switchScene();
        }
    }*/

    public void switchScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        Debug.Log("Switching scenes..");

        if (currentScene.name == "Supermarket")
        {
            DataLogger.Log("SceneSwap", "Changing to kitchen...");
            SceneManager.LoadScene("Scenes/Kitchen/Kitchen");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BasketRecipeSystem basketSystem = GameObject.FindGameObjectWithTag("BasketRecipeSystem").GetComponent<BasketRecipeSystem>();
        if (other.gameObject.layer == 6 && basketSystem.containsOneRecipe())
        {
            DataLogger.Log("SceneSwap", "User can sawp to kitchen.");
            switchScene();
        }
        else
        {
            DataLogger.Log("SceneSwap", "User tried to go to kitchen but has no recipes");
        }
    }
}
