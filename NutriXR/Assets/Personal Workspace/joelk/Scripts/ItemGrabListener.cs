using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ItemGrabListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Display food item UI only when being grabbed
        var handGrabInteractable = gameObject.GetComponent<HandGrabInteractable>();
        if (InteractableState.Select == handGrabInteractable.State)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag("FoodItemCanvas"))
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag("FoodItemCanvas"))
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
