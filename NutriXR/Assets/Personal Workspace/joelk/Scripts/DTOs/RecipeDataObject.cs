using System;
using System.Collections.Generic;

[Serializable]
    public class RecipeDataObject
    {
        public int id;
        public string name;

        public RecipeIngredientCategory[] inputIngredientCategories;

        [Serializable]
        public class RecipeIngredientCategory
        {
            public int id;
            public float ingredientWeight;
            public bool isOptional = false;
        }
    }

