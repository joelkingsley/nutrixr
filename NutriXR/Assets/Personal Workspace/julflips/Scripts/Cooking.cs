using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Cooking : MonoBehaviour
{
    [SerializeField] private List<String> potIngredients; //ToDo: Remove [SerializeField]
    [SerializeField] private JSONReader jsonReader;
    [SerializeField] private GameObject Content;
    [SerializeField] private GameObject potUIElementPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            addIngredient(other.gameObject);

            List<JSONReader.Recipe> possibleRecipes = checkRecipies();
            /*foreach (JSONReader.Recipe recipe in possibleRecipes)
            {
                Debug.Log(recipe.name);
            }*/
        }
    }

    private void addIngredient(GameObject ingredient)
    {
        Debug.Log("Add " + ingredient.GetComponent<Ingredient>().fdcName + " to Pot");
        GameObject UIElement = Instantiate(potUIElementPrefab, Content.transform);
        Quaternion oldRot = ingredient.transform.rotation;
        ingredient.transform.SetParent(UIElement.transform.GetChild(2), false);
        ingredient.transform.localPosition = new Vector3();
        ingredient.transform.rotation = oldRot;
        ingredient.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        potIngredients.Add(ingredient.GetComponent<Ingredient>().fdcName);
    }

    private void removeIngredient()
    {

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
