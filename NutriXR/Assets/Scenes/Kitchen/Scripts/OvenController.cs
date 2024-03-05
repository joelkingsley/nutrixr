using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class OvenController : MonoBehaviour
{

    [SerializeField] private GameObject OvenDoor;
    [SerializeField] private GameObject OvenTray;

    private bool isGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float current_rot = OvenDoor.transform.eulerAngles.x;
        int threshold = 48; //Degree of the oven door at which the tray closes automatically (Minimum Angle the door needs to be opened for the tray to be fully extended)

        if (current_rot < threshold)
        {
            //Calculate the maximum distance the oven tray can be extended at the given door angle
            float ext_distance = Math.Min((current_rot / threshold) * 0.4f, OvenTray.transform.localPosition.z);
            OvenTray.transform.localPosition = new Vector3(OvenTray.transform.localPosition.x, OvenTray.transform.localPosition.y, ext_distance);
            OvenTray.GetComponentInChildren<Grabbable>().enabled = false;

            //if not grabbed: close automatically
            if (!isGrabbed && current_rot > 1)
            {
                current_rot -= Time.deltaTime * 10;
                OvenDoor.transform.eulerAngles = new Vector3(current_rot, OvenDoor.transform.eulerAngles.y, OvenDoor.transform.eulerAngles.z);
            }
        }
        else
        {
            OvenTray.GetComponentInChildren<Grabbable>().enabled = true;
            //if not grabbed: open automatically
            if (!isGrabbed && current_rot < 99)
            {
                current_rot += Time.deltaTime * 10;
                OvenDoor.transform.eulerAngles = new Vector3(current_rot, OvenDoor.transform.eulerAngles.y, OvenDoor.transform.eulerAngles.z);
            }
        }
    }

    public void GrabStart()
    {
        isGrabbed = true;
    }

    public void GrabEnd()
    {
        isGrabbed = false;
    }
}
