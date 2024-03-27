using System;
using System.Collections.Generic;
using System.Linq;
using Personal_Workspace.joelk.DTOs;
using UnityEngine;

namespace Personal_Workspace.joelk
{
    public class JsonReader : MonoBehaviour
    {
        public RecipeList recipeList;
        public IngredientCategoryList ingredientCategoryList;
        public IngredientChoiceList ingredientChoiceList;
        public List<IngredientItemData> ingredientItems;
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

            ingredientItems =
                ingredientChoiceList.ingredientChoices.Select(choice => new IngredientItemData(choice)).ToList();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
