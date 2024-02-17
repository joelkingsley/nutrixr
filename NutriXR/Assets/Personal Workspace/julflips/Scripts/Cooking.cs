using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cooking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<String> ingredients = new List<String>();
    [SerializeField] private JSONReader jsonReader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            Debug.Log("Add " + other.gameObject.name + " to Pot");
            ingredients.Add(other.gameObject.name);
            Destroy(other.gameObject);
            checkRecipies();
        }
    }

    private void checkRecipies()
    {
        List<JSONReader.Recipe> validRecipies = new List<JSONReader.Recipe>(jsonReader.recipes);

    }
}
