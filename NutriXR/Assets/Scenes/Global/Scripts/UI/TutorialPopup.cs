using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private TutorialPopup nextPopup = null;

    public void ButtonPressed()
    {
        if (nextPopup != null)
        {
            nextPopup.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
