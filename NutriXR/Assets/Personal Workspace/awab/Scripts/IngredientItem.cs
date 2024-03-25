using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using UnityEditor;
using TMPro;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class IngredientItem : MonoBehaviour
{
    private IngredientItemData data;
    private BasketSystem _basketSystemSystem;
    public string fdcName;

    public TMP_Text nameComponent;
    public TMP_Text proteinTextComponent;
    public TMP_Text carbohydratesTextComponent;
    public TMP_Text fatsTextComponent;
    public TMP_Text sugarTextComponent;
    public TMP_Text caloriesTextComponent;


    public IngredientItemData data;
    private ShoppingCart _shoppingCartSystem;
    private DataStorage dataStorage;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;
    private bool isInCart = false;
    private bool isInPot = false;
    private Collider[] allColliders;
    private float inPotScaleModifier = 0.5f;

    [SerializeField]
    public string fdcName;

    [SerializeField]
    public bool spawnInFridge;

    // Start is called before the first frame update
    void Start()
    {
        _basketSystemSystem = GameObject.FindGameObjectWithTag("BasketSystem").GetComponent<BasketSystem>();
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
        startingScale = transform.localScale;

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

        if (other.gameObject.CompareTag("PotEntry") && !isInPot)
        {
            other.transform.GetComponentInParent<BasketSystem>().AddToCart(this);
            transform.localScale *= inPotScaleModifier;
            transform.SetParent(other.GetComponentInParent<Transform>(), true);
            isInPot = true;
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

        if (other.gameObject.CompareTag("PotExit") && isInPot)
        {
            transform.parent = null;
            transform.localScale = startingScale;
            other.transform.GetComponentInParent<BasketSystem>().RemoveFromCart(this);
            isInPot = false;
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
