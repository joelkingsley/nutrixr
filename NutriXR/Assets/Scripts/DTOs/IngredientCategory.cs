using System;
using System.Collections.Generic;

namespace DTOs
{
    [Serializable]
    public class IngredientCategory
    {
        /// Unique identifier of a ingredient category
        public int categoryId;
        /// Name of the ingredient category (Eg. Oil, Milk, Meat)
        public string name;
        /// List of uniquely identified names (from FDC) of the ingredient choices that belong to this ingredient category
        public List<string> fdcNamesOfIngredientChoices;
    }
}
