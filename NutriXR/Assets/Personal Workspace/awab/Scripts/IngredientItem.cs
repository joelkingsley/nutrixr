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
    [SerializeField] private HapticClip hapticClipBackBag;
    private HapticClipPlayer _hapticClipPlayer;
    public string fdcName;
    public IngredientItemData data;
    private AudioSource _audioClipPlayer;
    private GameObject _player;
    private DataStorage dataStorage;
    private bool _readyForSelection;

     void Awake()
     {
         _hapticClipPlayer = new HapticClipPlayer(hapticClipBackBag);
         _audioClipPlayer = GetComponent<AudioSource>();
     }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);
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
        if (!other.gameObject.CompareTag("Player"))
        {
            var o = other.gameObject;
            Debug.Log("other trigger name:" + o.name + "tag: " + o.tag);
            return;
        }

        if (GetComponentInChildren<GrabInteractable>().State == InteractableState.Select)
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
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (GetComponentInChildren<GrabInteractable>().State == InteractableState.Select)
        {
            _readyForSelection = false;
            return;
        }
        if (_readyForSelection)
        {
            SelectFoodItem();
        }


    }

    public void SelectFoodItem()
    {
        _player.GetComponent<Basket>().AddToBasket(this);
        gameObject.SetActive(false);
    }
}
