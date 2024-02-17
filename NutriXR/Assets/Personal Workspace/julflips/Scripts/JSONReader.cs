using System;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset Recipejson;
    public TextAsset Ingredientjson;
    public TextAsset Categoryjson;

    [Serializable]
    public class Recipe
    {
        public string id;
        public string name;

        public List<int> ingredientCategories;
    }

    [Serializable]
    public class Recipes
    {
        public Recipe[] recipes;
    }

    [Serializable]
    public class Ingredient
    {
        [Serializable]
        public class Data
        {
            public String foodClass;
            public String description;
            //ToDo: Parse Nutrients
        }
        public String fdcName;
        public Data data;
    }

    [Serializable]
    public class Ingredients
    {
        public Ingredient[] ingredientChoices;
    }

    [Serializable]
    public class Category
    {
        public int id;
        public string name;
        public String[] fdcNamesOfIngredientChoices;
    }

    [Serializable]
    public class Categories
    {
        public Category[] ingredientCategories;
    }

    public Recipes recipes;
    public Ingredients ingredients;
    public Categories categories;

    void Start()
    {
        recipes = JsonUtility.FromJson<Recipes>(Recipejson.text);
        /*foreach (Recipe recipe in recipes.recipes)
        {
            Debug.Log("Found recipe: " + recipe.id + " " + recipe.name);
        }*/

        ingredients = JsonUtility.FromJson<Ingredients>(Ingredientjson.text);
        /*foreach (Ingredient ingredient in ingredients.ingredientChoices)
        {
            Debug.Log("Found ingredient: " + ingredient.fdcName + " | " + ingredient.data.description + " | " + ingredient.data.foodClass);
        }*/

        categories = JsonUtility.FromJson<Categories>(Categoryjson.text);
        /*foreach (Category category in categories.ingredientCategories)
        {
            Debug.Log("Found category: " + category.id + " | " + category.name + " | " + category.fdcNamesOfIngredientChoices);
        }*/
    }
}
