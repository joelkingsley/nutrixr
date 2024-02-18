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
    private class Recipes
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
    private class Ingredients
    {
        public Ingredient[] ingredientChoices;
    }

    [Serializable]
    public class Category
    {
        public int categoryId;
        public string name;
        public String[] fdcNamesOfIngredientChoices;
    }

    [Serializable]
    private class Categories
    {
        public Category[] ingredientCategories;
    }

    public Recipe[] recipes;
    public Dictionary<String, Ingredient> ingredients = new Dictionary<String, Ingredient>();
    public Dictionary<int, Category> categories = new Dictionary<int, Category>();

    void Start()
    {
        recipes = JsonUtility.FromJson<Recipes>(Recipejson.text).recipes;
        /*foreach (Recipe recipe in recipes.recipes)
        {
            Debug.Log("Found recipe: " + recipe.id + " " + recipe.name);
        }*/

        foreach (Ingredient ingredient in JsonUtility.FromJson<Ingredients>(Ingredientjson.text).ingredientChoices)
        {
            ingredients.Add(ingredient.fdcName, ingredient);
        }
        /*foreach (String key in ingredients.Keys)
        {
            Debug.Log("Found ingredient: " + key + " | " + ingredients[key].data.description + " | " + ingredients[key].data.foodClass);
        }*/

        foreach (Category category in JsonUtility.FromJson<Categories>(Categoryjson.text).ingredientCategories)
        {
            categories.Add(category.categoryId, category);
        }
        /*foreach (int key in categories.Keys)
        {
            Debug.Log("Found category: " + key + " | " + categories[key].name + " | " + categories[key].fdcNamesOfIngredientChoices);
        }*/
    }
}
