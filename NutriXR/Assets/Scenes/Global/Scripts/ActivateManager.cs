using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateManager : MonoBehaviour
{
    [SerializeField] private OVRManager _ovrManager;

    [SerializeField]
    private GameObject _OVRControllers;

    [SerializeField]
    private GameObject _OVRHands;

    [SerializeField]
    private GameObject _OVRControllerDrivenHands;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
            _ovrManager.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.None;
            _OVRControllers.SetActive(true);
            _OVRHands.SetActive(true);
            _OVRControllerDrivenHands.SetActive(false);
        #else
            _ovrManager.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.Natural;
            _OVRControllers.SetActive(false);
            _OVRHands.SetActive(false);
            _OVRControllerDrivenHands.SetActive(true);
        #endif
    }
}
