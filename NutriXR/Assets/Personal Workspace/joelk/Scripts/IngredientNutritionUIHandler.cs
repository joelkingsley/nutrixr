using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Personal_Workspace.joelk.DTOs;
using TMPro;
using UnityEngine;


    public class IngredientNutritionUIHandler : MonoBehaviour
    {
        public TMP_Text nameComponent;
        public TMP_Text proteinTextComponent;
        public TMP_Text carbohydratesTextComponent;
        public TMP_Text fatsTextComponent;
        public TMP_Text sugarTextComponent;
        public TMP_Text caloriesTextComponent;

        public RecipeList recipeList;
        public IngredientCategoryList ingredientCategoryList;
        public IngredientChoiceList ingredientChoiceList;
        public TextAsset recipeJson;
        public TextAsset ingredientCategoryJson;
        public TextAsset ingredientChoiceJson;

        [Serializable]
        public class RecipeList
        {
            public List<RecipeDataObject> recipes;
        }

        [Serializable]
        public class IngredientCategoryList
        {
            public List<IngredientCategory> ingredientCategories;
        }

        [Serializable]
        public class IngredientChoiceList
        {
            public List<IngredientDataObject> ingredientChoices;
        }

        // Start is called before the first frame update
        void Start()
        {
            recipeList = JsonUtility.FromJson<RecipeList>(recipeJson.text);
            ingredientCategoryList = JsonUtility.FromJson<IngredientCategoryList>(ingredientCategoryJson.text);
            ingredientChoiceList = JsonUtility.FromJson<IngredientChoiceList>(ingredientChoiceJson.text);

            // Choosing the first ingredient from a list of all ingredient (choices)
            var ingredientItems =
                ingredientChoiceList.ingredientChoices.Select(choice => new IngredientItemData(choice)).ToList();
            Debug.Log(ingredientItems.First().protein);

            nameComponent.text = ingredientItems.First().name;
            proteinTextComponent.text = $"{ingredientItems.First().protein} g";
            carbohydratesTextComponent.text = $"{ingredientItems.First().carbohydrates} g";
            fatsTextComponent.text = $"{ingredientItems.First().fat} g";
            sugarTextComponent.text = $"{ingredientItems.First().sugar} g";
            caloriesTextComponent.text = $"{ingredientItems.First().caloriesInKcal} kcal";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

