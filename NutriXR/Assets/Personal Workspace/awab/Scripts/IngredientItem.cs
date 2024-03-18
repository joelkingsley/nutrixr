using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class IngredientItem : MonoBehaviour
{
    private IngredientItemData data;
    private BasketSystem _basketSystemSystem;
    private DataStorage dataStorage;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private bool isInCart = false;
    private Collider[] allColliders;

    [SerializeField]
    public string fdcName;

    // Start is called before the first frame update
    void Start()
    {
        _basketSystemSystem = GameObject.FindGameObjectWithTag("BasketSystem").GetComponent<BasketSystem>();
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        allColliders = GetComponentsInChildren<Collider>(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect()
    {
        foreach (Collider c in allColliders)
        {
            c.gameObject.layer = LayerMask.NameToLayer("SelectedIngredientItem");
        }
    }

    public void OnUnselect()
    {
        foreach (Collider c in allColliders)
        {
            c.gameObject.layer = LayerMask.NameToLayer("UnselectedIngredientItem");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart") && !isInCart)
        {
            transform.SetParent(other.gameObject.transform, true);
            _basketSystemSystem.AddToCart(this);
            isInCart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart") && isInCart)
        {
            transform.parent = null;
            _basketSystemSystem.RemoveFromCart(this);
            isInCart = false;
        }
    }

    public void RespawnToStart()
    {
        gameObject.transform.position = startingPosition;
        gameObject.transform.rotation = startingRotation;
    }

    public IngredientItemData GetIngredientItemData()
    {
        return data;
    }
}
