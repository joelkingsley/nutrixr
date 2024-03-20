using System;
using System.Collections.Generic;
using System.Linq;
using Personal_Workspace.joelk.DTOs;
using Personal_Workspace.joelk.Entities;
using TMPro;
using UnityEngine;

namespace Personal_Workspace.joelk.Scripts
{
    public class RecipeNutritionUIHandler: MonoBehaviour
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

            // Choosing a permutation of ingredient selection for the first recipe
            var firstRecipe = recipeList.recipes.First();
            List<(IngredientItemData, float)> ingredientItemsWithWeights = new List<(IngredientItemData, float)>();
            foreach (var inputIngredientCategory in firstRecipe.inputIngredientCategories)
            {
                var categoryId = inputIngredientCategory.id;
                var category = ingredientCategoryList.ingredientCategories.Find(x => x.categoryId == categoryId);
                var firstIngredientChoiceString = category.fdcNamesOfIngredientChoices.First();
                var ingredientChoice =
                    ingredientChoiceList.ingredientChoices.Find(x => x.fdcName == firstIngredientChoiceString);
                var ingredientItemData = new IngredientItemData(ingredientChoice);
                ingredientItemsWithWeights.Add((ingredientItemData, inputIngredientCategory.ingredientWeight));
            }
            /*var recipeItem = new RecipeItemData(firstRecipe.id, firstRecipe.name, ingredientItemsWithWeights);

            nameComponent.text = recipeItem.name;
            proteinTextComponent.text = $"{recipeItem.protein} g";
            carbohydratesTextComponent.text = $"{recipeItem.carbohydrates} g";
            fatsTextComponent.text = $"{recipeItem.fat} g";
            sugarTextComponent.text = $"{recipeItem.sugar} g";
            caloriesTextComponent.text = $"{recipeItem.caloriesInKcal} kcal";*/
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
