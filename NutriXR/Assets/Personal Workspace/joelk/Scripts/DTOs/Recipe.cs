using System;
using System.Collections.Generic;

namespace Personal_Workspace.joelk.DTOs
{
    [Serializable]
    public class Recipe
    {
        public int id;
        public string name;

        public RecipeIngredientCategory[] inputIngredientCategories;

        [Serializable]
        public class RecipeIngredientCategory
        {
            public int id;
            public int ingredientWeight;
            public bool isOptional = false;
        }
    }
}
