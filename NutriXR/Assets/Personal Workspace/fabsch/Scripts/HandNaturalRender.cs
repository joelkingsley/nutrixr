using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandNatuaralRender : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer _handLeft;

    [SerializeField]
    private SkinnedMeshRenderer _handRight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // no real performance down grade, the skinnedmeshrenderers are set to enabled/disabled every frame as well
    void Update()
    {
        if (_handLeft == null || _handRight == null || OVRManager.instance == null)
        {
            return;
        }

        if (!_handLeft.enabled && !_handRight.enabled)
        {
            OVRManager.instance.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.ConformingToController;
        }
        else
        {
            OVRManager.instance.controllerDrivenHandPosesType = OVRManager.ControllerDrivenHandPosesType.Natural;
        }
    }
}
