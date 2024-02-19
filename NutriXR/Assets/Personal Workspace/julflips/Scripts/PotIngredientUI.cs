using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotIngredientUI : MonoBehaviour
{
    public GameObject surface;
    private float surfaceWidth = 0.4f;
    private Vector3 initialScale;
    private float scaleFactor = 30;

    private void Start()
    {
        initialScale = transform.localScale;
    }


    void Update()
    {
        if (transform.hasChanged)
        {
            float distanceToBorder = Mathf.Abs(transform.position.x - surface.transform.position.x) - (surfaceWidth / 2 - 0.06f);
            transform.localScale = initialScale * Math.Clamp(1 - distanceToBorder * scaleFactor, 0, 1);
        }
    }
}
