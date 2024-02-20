using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotIngredientUI : MonoBehaviour
{
    private float surfaceWidth = 0.4f;
    private Vector3 initialScale;
    private float scaleFactor = 30;
    private Vector3 center;

    private void Start()
    {
        initialScale = transform.localScale;
    }


    void Update()
    {
        float distanceToBorder = Mathf.Abs(transform.position.x - 0.5624f) - (surfaceWidth / 2 - 0.06f);
        float newScalePercentage = Math.Clamp(1 - distanceToBorder * scaleFactor, 0, 1);
        if (newScalePercentage == 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.localScale = initialScale * newScalePercentage;
        }

        /*if (transform.hasChanged)
        {
            float distanceToBorder = Mathf.Abs(transform.position.x - 0.5624f) - (surfaceWidth / 2 - 0.06f);
            transform.localScale = initialScale * Math.Clamp(1 - distanceToBorder * scaleFactor, 0, 1);
        }*/
    }
}
