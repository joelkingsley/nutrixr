using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<String> potIngredients; //ToDo: Remove [SerializeField]
    [SerializeField] private JSONReader jsonReader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            Debug.Log("Add " + other.gameObject.GetComponent<Ingredient>().fdcName + " to Pot");
            potIngredients.Add(other.gameObject.GetComponent<Ingredient>().fdcName);
            Destroy(other.gameObject);

            List<JSONReader.Recipe> possibleRecipes = checkRecipies();
            /*foreach (JSONReader.Recipe recipe in possibleRecipes)
            {
                Debug.Log(recipe.name);
            }*/
        }
    }

    private List<JSONReader.Recipe> checkRecipies()
    {
        List<JSONReader.Recipe> validRecipies = new List<JSONReader.Recipe>();

        foreach (JSONReader.Recipe recipe in jsonReader.recipes)
        {
            bool isValidRecipe = true;
            foreach (String potIngredient in potIngredients)
            {
                bool isInRecipe = false;
                foreach (int categoryID in recipe.ingredientCategories)
                {
                    if (jsonReader.categories[categoryID].fdcNamesOfIngredientChoices.Contains(potIngredient))
                    {
                        isInRecipe = true;
                    }
                }
                if (!isInRecipe)
                {
                    isValidRecipe = false;
                }
            }

            if (isValidRecipe)
            {
                validRecipies.Add(recipe);
            }
        }
        return validRecipies;
    }
}
