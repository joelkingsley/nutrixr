using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class OvenController : MonoBehaviour
{

    [SerializeField] private GameObject OvenDoor;
    [SerializeField] private GameObject OvenTray;

    [SerializeField] private GameObject TrayInteractable;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float current_rot = OvenDoor.transform.eulerAngles.x;
        int threshold = 48;

        if (current_rot < threshold)
        {
            float test = (current_rot / threshold) * 0.4f;
            OvenTray.transform.localPosition = new Vector3(OvenTray.transform.localPosition.x, OvenTray.transform.localPosition.y, Math.Min(OvenTray.transform.localPosition.z, test));
            TrayInteractable.SetActive(false);

            //if not grabbed: close automatically

        }
        else
        {
            TrayInteractable.SetActive(true);
            //if not grabbed: open automatically
        }
    }
}
