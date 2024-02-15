using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    private OVRGrabber _ovrGrabber;
    // Start is called before the first frame update
    void Start()
    {
        _ovrGrabber = GetComponent<OVRGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            if (_ovrGrabber.grabbedObject != null)
            {
                SelectItem(_ovrGrabber.grabbedObject.gameObject);
            }
        }
    }

    private void SelectItem(GameObject grabbedGameObject)
    {
        var data = grabbedGameObject.GetComponent<NutritionalData>().GetAll();
        Debug.Log(data);
        Destroy(grabbedGameObject);
    }
}
