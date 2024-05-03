using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupHandler : MonoBehaviour
{
    [SerializeField] public GameObject nextPopup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideElement()
    {
        gameObject.SetActive(false);
        if (nextPopup != null)
        {
            nextPopup.SetActive(true);
        }
    }
}
