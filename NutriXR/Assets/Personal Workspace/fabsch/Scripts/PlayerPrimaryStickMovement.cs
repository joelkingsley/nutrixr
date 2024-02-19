using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMovement : MonoBehaviour
{
    public OVRCameraRig CameraRig;
    public float Speed = 1.5f;

    private void Awake()
    {
        if (CameraRig == null) CameraRig = GetComponent<OVRCameraRig>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // we need the orientation of the center eye anchor and the OVRCameraRig element itself
        Quaternion ortEyeAnchor = CameraRig.centerEyeAnchor.rotation;
        Vector3 ortEulerEyeAnchor = ortEyeAnchor.eulerAngles;
        ortEulerEyeAnchor.z = ortEulerEyeAnchor.x = 0f;

        Quaternion ortOvrCameraRig = CameraRig.gameObject.transform.rotation;
        Vector3 ortEulerOvrCameraRig = ortOvrCameraRig.eulerAngles;
        ortEulerOvrCameraRig.z = ortEulerOvrCameraRig.x = 0f;
        ortEulerOvrCameraRig.y *= -1; // somehow required lol

        Quaternion ort = Quaternion.Euler(ortEulerEyeAnchor) * Quaternion.Euler(ortEulerOvrCameraRig);

        Vector3 moveDir = Vector3.zero;
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        moveDir += (ort) * (primaryAxis.x * Vector3.right);
        moveDir += (ort) * (primaryAxis.y * Vector3.forward);

        if (primaryAxis.x != 0 || primaryAxis.y != 0)
        {
            UnityEngine.Debug.Log(ort.eulerAngles);
            transform.Translate(moveDir * (Speed * Time.fixedDeltaTime));
        }
    }
}
