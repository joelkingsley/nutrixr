using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        String fdcName = ingredient.GetComponent<Ingredient>().fdcName;
        Debug.Log("Add " + fdcName + " to Pot");
        GameObject UIElement = Instantiate(potUIElementPrefab, Content.transform);
        UIElement.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = fdcName;
        Quaternion oldRot = ingredient.transform.rotation;
        ingredient.transform.SetParent(UIElement.transform.GetChild(2), false);
        ingredient.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ingredient.transform.rotation = oldRot;
        ingredient.transform.localPosition = new Vector3();
        ingredient.GetComponent<Rigidbody>().velocity = new Vector3();
        potIngredients.Add(fdcName);
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
