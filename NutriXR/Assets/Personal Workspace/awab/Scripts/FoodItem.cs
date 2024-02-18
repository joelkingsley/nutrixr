using System;
using System.Collections;
using System.Collections.Generic;
using Meta.Voice.Audio;
using Oculus.Haptics;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [SerializeField] private HapticClip hapticClipBackBag;
    [SerializeField] private AudioClip audioClipBackBag;
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
        if (!other.gameObject.CompareTag("Player")) return;
        if (GetComponent<OVRGrabbable>().grabbedBy.GetComponent<ItemSelector>().controller ==
            OVRInput.Controller.LTouch)
        {
            _hapticClipPlayer.Play(Oculus.Haptics.Controller.Left);
            _audioClipPlayer.Play();
        }
        if (GetComponent<OVRGrabbable>().grabbedBy.GetComponent<ItemSelector>().controller ==
            OVRInput.Controller.RTouch)
        {
            _hapticClipPlayer.Play(Oculus.Haptics.Controller.Right);
            _audioClipPlayer.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (GetComponent<OVRGrabbable>().isGrabbed) return;
        SelectFoodItem();


    }

    public void SelectFoodItem()
    {
        Debug.Log(data.ToJson());
        gameObject.SetActive(false);
        Debug.Log("Deleting: "+gameObject.name);
    }
}
