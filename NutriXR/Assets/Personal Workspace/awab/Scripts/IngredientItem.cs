using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class IngredientItem : MonoBehaviour
{
    public string fdcName;

    public TMP_Text nameComponent;
    public TMP_Text proteinTextComponent;
    public TMP_Text carbohydratesTextComponent;
    public TMP_Text fatsTextComponent;
    public TMP_Text sugarTextComponent;
    public TMP_Text caloriesTextComponent;

     void Awake()
     {
         _hapticClipPlayer = new HapticClipPlayer(hapticClipBackBag);
         _audioClipPlayer = GetComponent<AudioSource>();
     }
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

        // To be moved to a separate script
        nameComponent.text = data.name;
        proteinTextComponent.text = $"{data.protein} g";
        carbohydratesTextComponent.text = $"{data.carbohydrates} g";
        fatsTextComponent.text = $"{data.fat} g";
        sugarTextComponent.text = $"{data.sugar} g";
        caloriesTextComponent.text = $"{data.caloriesInKcal} kcal";

        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
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
