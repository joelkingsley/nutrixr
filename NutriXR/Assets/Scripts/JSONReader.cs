using System;
using System.Collections;
using System.Collections.Generic;
using DTOs;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public RecipeList recipeList;
    public IngredientCategoryList ingredientCategoryList;
    public IngredientChoiceList ingredientChoiceList;
    public TextAsset recipeJson;
    public TextAsset ingredientCategoryJson;
    public TextAsset ingredientChoiceJson;

    [Serializable]
    public class RecipeList
    {
        public Recipe[] recipes;
    }

    [Serializable]
    public class IngredientCategoryList
    {
        public IngredientCategory[] ingredientCategories;
    }

    [Serializable]
    public class IngredientChoiceList
    {
        public IngredientChoice[] ingredientChoices;
    }

    // Start is called before the first frame update
    void Start()
    {
        recipeList = JsonUtility.FromJson<RecipeList>(recipeJson.text);
        ingredientCategoryList = JsonUtility.FromJson<IngredientCategoryList>(ingredientCategoryJson.text);
        ingredientChoiceList = JsonUtility.FromJson<IngredientChoiceList>(ingredientChoiceJson.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
