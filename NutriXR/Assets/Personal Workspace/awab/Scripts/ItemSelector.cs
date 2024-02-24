using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    private HandGrabInteractor _grabInteractor;

    public OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        _grabInteractor = GetComponent<HandGrabInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            if (_grabInteractor.State == InteractorState.Select)
            {
                SelectItem(_grabInteractor.SelectedInteractable.gameObject);
            }
        }
    }

    public void SelectItem(GameObject grabbedGameObject)
    {
        grabbedGameObject.GetComponentInParent<FoodItem>().SelectFoodItem();
    }
}
