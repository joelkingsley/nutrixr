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
    //[SerializeField] private List<String> potIngredients; //ToDo: Remove [SerializeField]
    //[SerializeField] private List<GameObject> potIngredients;
    [SerializeField] Dictionary<GameObject, Vector3> potIngredients = new Dictionary<GameObject, Vector3>();
    [SerializeField] private JSONReader jsonReader;
    [SerializeField] private GameObject Content;
    //[SerializeField] private GameObject potUIElementPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            other.tag = "InPot";
            addIngredient(other.gameObject);

            //List<JSONReader.Recipe> possibleRecipes = checkRecipies();

            /*foreach (JSONReader.Recipe recipe in possibleRecipes)
            {
                Debug.Log(recipe.name);
            }*/
            return;
        }

        if (other.gameObject.tag == "InPot")
        {
            other.tag = "Ingredient";
            removeIngredient(other.gameObject);

            //List<JSONReader.Recipe> possibleRecipes = checkRecipies();

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
        potIngredients.Add(ingredient, ingredient.transform.localScale);
        ingredient.transform.localScale *= 0.1f;

        /*GameObject UIElement = Instantiate(potUIElementPrefab, Content.transform);
        UIElement.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = fdcName;
        Quaternion oldRot = ingredient.transform.rotation;
        ingredient.GetComponentInChildren<Grabbable>().enabled = false;
        ingredient.transform.SetParent(UIElement.transform.GetChild(2), false);
        ingredient.transform.rotation = oldRot;
        ingredient.transform.localPosition = new Vector3();
        ingredient.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ingredient.GetComponent<Rigidbody>().velocity = new Vector3();
        potIngredients.Add(fdcName);*/
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
