using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionalData : MonoBehaviour
{

    public string itemName = "Test 1";

    public int calories = 123;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetAll()
    {
        return "{\"" + itemName + "\": " + calories + "}";
    }
}
