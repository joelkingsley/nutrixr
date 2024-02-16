using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public Button done;

    public GameObject uiGameObject;

    public GameObject interactor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoneIsClicked()
    {
        Destroy(uiGameObject);
        Destroy(interactor);
    }
}
