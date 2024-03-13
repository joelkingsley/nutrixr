using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Cooking : MonoBehaviour
{
    [SerializeField] private Dictionary<GameObject, Vector3> potIngredients = new Dictionary<GameObject, Vector3>();
    [SerializeField] private JSONReader jsonReader;
    [SerializeField] private GameObject Content;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ingredient"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("PotIngredient");
            addIngredient(other.gameObject);

            //List<JSONReader.Recipe> possibleRecipes = checkRecipies();

            /*foreach (JSONReader.Recipe recipe in possibleRecipes)
            {
                Debug.Log(recipe.name);
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PotIngredient"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Ingredient");
            removeIngredient(other.gameObject);
        }
    }

    private void addIngredient(GameObject ingredient)
    {
        String fdcName = ingredient.GetComponent<Ingredient>().fdcName;
        Debug.Log("Add " + fdcName + " to Pot");
        potIngredients.Add(ingredient, ingredient.transform.localScale);
        ingredient.transform.localScale *= 0.5f;
    }

    private void removeIngredient(GameObject ingredient)
    {
        String fdcName = ingredient.GetComponent<Ingredient>().fdcName;
        Debug.Log("Remove " + fdcName + " from Pot");
        ingredient.transform.localScale = potIngredients[ingredient];
        potIngredients.Remove(ingredient);
    }

    /*private List<JSONReader.Recipe> checkRecipies()
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
    }*/
}
