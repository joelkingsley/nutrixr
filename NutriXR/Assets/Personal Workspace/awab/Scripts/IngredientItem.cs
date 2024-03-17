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
    [SerializeField] private HapticClip hapticClipBackBag;
    private HapticClipPlayer _hapticClipPlayer;
    public string fdcName;
    public IngredientItemData data;
    private AudioSource _audioClipPlayer;
    private Basket _basketSystem;
    private DataStorage dataStorage;
    private bool _readyForSelection;

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

    // Start is called before the first frame update
    void Start()
    {
        _basketSystem = GameObject.FindWithTag("BasketSystem").GetComponent<Basket>();
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);

        nameComponent.text = data.name;
        proteinTextComponent.text = $"{data.protein} g";
        carbohydratesTextComponent.text = $"{data.carbohydrates} g";
        fatsTextComponent.text = $"{data.fat} g";
        sugarTextComponent.text = $"{data.sugar} g";
        caloriesTextComponent.text = $"{data.caloriesInKcal} kcal";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        _hapticClipPlayer.Dispose();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("ShoppingCart"))
        {
            var o = other.gameObject;
            Debug.Log("other trigger name:" + o.name + "tag: " + o.tag);
            return;
        }

        /*if (GetComponentInChildren<GrabInteractable>().State == InteractableState.Select)
        {
            var interactors = GetComponentInChildren<GrabInteractable>().SelectingInteractors;
            if (interactors != null && interactors.Count != 0 && interactors.First().GetComponent<ItemSelector>().controller ==
                OVRInput.Controller.LTouch)
            {
                _hapticClipPlayer.Play(Oculus.Haptics.Controller.Left);

            }
            if (interactors != null && interactors.Count != 0 && interactors.First().GetComponent<ItemSelector>().controller ==
                OVRInput.Controller.RTouch)
            {
                _hapticClipPlayer.Play(Oculus.Haptics.Controller.Right);

            }
            _audioClipPlayer.Play();
            _readyForSelection = true;
        }*/

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("ShoppingCart")) return;
        /*if (GetComponentInChildren<GrabInteractable>().State == InteractableState.Select)
        {
            _readyForSelection = false;
            return;
        }*/
        //if (_readyForSelection)
        //{
            SelectFoodItem();
        //}


    }

    public void SelectFoodItem()
    {
        _basketSystem.AddToBasket(this);
        // gameObject.SetActive(false);
    }
}
