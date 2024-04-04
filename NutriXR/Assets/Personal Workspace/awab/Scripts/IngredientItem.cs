using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using TMPro;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class IngredientItem : MonoBehaviour
{
    public string fdcName;

    private TMP_Text _nameComponent;
    private TMP_Text _proteinTextComponent;
    private TMP_Text _carbohydratesTextComponent;
    private TMP_Text _fatsTextComponent;
    private TMP_Text _sugarTextComponent;
    private TMP_Text _caloriesTextComponent;

    private HandGrabInteractable _handGrabInteractable;

    public IngredientItemData data;
    private ShoppingCart _shoppingCartSystem;
    private DataStorage dataStorage;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private bool isInCart = false;

    // Start is called before the first frame update
    void Start()
    {
        _shoppingCartSystem = GameObject.FindGameObjectWithTag("BasketSystem").GetComponent<ShoppingCart>();
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);
        _handGrabInteractable = gameObject.GetComponent<HandGrabInteractable>();

        // Setup text fields
        var textFields = gameObject.GetComponentsInChildren<TMP_Text>();
        _nameComponent = textFields.First(x => x.gameObject.name == "Name");
        _proteinTextComponent = textFields.First(x => x.gameObject.name == "ProteinValue");
        _carbohydratesTextComponent = textFields.First(x => x.gameObject.name == "CarbohydratesValue");
        _fatsTextComponent = textFields.First(x => x.gameObject.name == "FatsValue");
        _sugarTextComponent = textFields.First(x => x.gameObject.name == "SugarValue");
        _caloriesTextComponent = textFields.First(x => x.gameObject.name == "CaloriesValue");

        // Populate text fields
        _nameComponent.text = data.name;
        _proteinTextComponent.text = $"{data.protein} g";
        _carbohydratesTextComponent.text = $"{data.carbohydrates} g";
        _fatsTextComponent.text = $"{data.fat} g";
        _sugarTextComponent.text = $"{data.sugar} g";
        _caloriesTextComponent.text = $"{data.caloriesInKcal} kcal";

        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (InteractableState.Select == _handGrabInteractable.State)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag("ItemCanvas"))
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag("ItemCanvas"))
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart") && !isInCart)
        {
            transform.SetParent(other.gameObject.transform, true);
            _shoppingCartSystem.AddToCart(this);
            isInCart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShoppingCart") && isInCart)
        {
            transform.parent = null;
            _shoppingCartSystem.RemoveFromCart(this);
            isInCart = false;
        }
    }

    public void RespawnToStart()
    {
        gameObject.transform.position = startingPosition;
        gameObject.transform.rotation = startingRotation;
    }


}
