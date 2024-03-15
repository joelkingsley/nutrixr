using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class IngredientItem : MonoBehaviour
{
    public string fdcName;

    public IngredientItemData data;
    private Basket basketSystem;
    private DataStorage dataStorage;

    // Start is called before the first frame update
    void Start()
    {
        DataStorage ds = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = ds.ReadIngredientData(fdcName);

        basketSystem = GameObject.FindWithTag("BasketSystem").GetComponent<Basket>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart"))
        {
            transform.SetParent(other.gameObject.transform, true);
            basketSystem.AddToBasket(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart"))
        {
            transform.parent = null;
        }
    }
}
