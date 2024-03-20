using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMovement : MonoBehaviour
{
    private CharacterController Controller;
    private OVRCameraRig CameraRig;
    private float playerSpeed = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        CameraRig = GetComponent<OVRCameraRig>();
    }

    private void Update()
    {
        // we need the orientation of the center eye anchor and the OVRCameraRig element itself
        Quaternion ortEyeAnchor = CameraRig.centerEyeAnchor.localRotation;
        Vector3 ortEulerEyeAnchor = ortEyeAnchor.eulerAngles;
        ortEulerEyeAnchor.z = ortEulerEyeAnchor.x = 0f;

        Quaternion ortOvrCameraRig = CameraRig.gameObject.transform.localRotation;
        Vector3 ortEulerOvrCameraRig = ortOvrCameraRig.eulerAngles;
        ortEulerOvrCameraRig.z = ortEulerOvrCameraRig.x = 0f;
        // ortEulerOvrCameraRig.y *= -1; // somehow required lol

        Quaternion ort = Quaternion.Euler(ortEulerEyeAnchor) * Quaternion.Euler(ortEulerOvrCameraRig);

        Vector3 moveDir = Vector3.zero;
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        moveDir += (ort) * (primaryAxis.x * Vector3.right);
        moveDir += (ort) * (primaryAxis.y * Vector3.forward);
        Controller.Move(moveDir * (playerSpeed * Time.deltaTime));
    }

}
