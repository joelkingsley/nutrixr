using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.Audio;
using Oculus.Haptics;
using Oculus.Interaction;
using UnityEngine;
using InteractableState = Oculus.Interaction.InteractableState;

public class FoodItem : MonoBehaviour
{
    [SerializeField] private HapticClip hapticClipBackBag;
    private HapticClipPlayer _hapticClipPlayer;
    public FoodItemData data;
    private AudioSource _audioClipPlayer;


     void Awake()
     {
         _hapticClipPlayer = new HapticClipPlayer(hapticClipBackBag);
         _audioClipPlayer = GetComponent<AudioSource>();
     }

    // Start is called before the first frame update
    void Start()
    {

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

        var interactors = GetComponentInChildren<GrabInteractable>().SelectingInteractors;
        if (interactors != null && interactors.Count != 0 && interactors.First().GetComponent<ItemSelector>().controller ==
            OVRInput.Controller.LTouch)
        {
            _hapticClipPlayer.Play(Oculus.Haptics.Controller.Left);
            _audioClipPlayer.Play();
        }
        if (interactors != null && interactors.Count != 0 && interactors.First().GetComponent<ItemSelector>().controller ==
            OVRInput.Controller.RTouch)
        {
            _hapticClipPlayer.Play(Oculus.Haptics.Controller.Right);
            _audioClipPlayer.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (GetComponentInChildren<GrabInteractable>().State == InteractableState.Select) return;
        SelectFoodItem();


    }

    public void SelectFoodItem()
    {
        Debug.Log(data.ToJson());
        Debug.Log("Deleting: "+gameObject.name);
        gameObject.SetActive(false);
    }
}
