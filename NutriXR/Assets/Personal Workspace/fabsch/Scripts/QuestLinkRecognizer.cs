using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLinkRecognizer : MonoBehaviour
{
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
            OVRManager.instance.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.ConformingToController;
            _OVRControllers.SetActive(true);
            _OVRHands.SetActive(false);
            _OVRControllerDrivenHands.SetActive(false);
        #else
            OVRManager.instance.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.Natural;
            _OVRControllers.SetActive(false);
            _OVRHands.SetActive(false);
            _OVRControllerDrivenHands.SetActive(true);
        #endif
    }
}
