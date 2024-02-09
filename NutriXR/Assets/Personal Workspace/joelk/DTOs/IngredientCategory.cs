using System;
using System.Collections.Generic;

namespace Personal_Workspace.joelk.DTOs
{
    [Serializable]
    public class IngredientCategory
    {
        public string categoryId;
        public string name;
        public List<string> fdcNamesOfIngredientChoices;
        public List<int> idsOfRecipesThatHaveThisCategory;
    }
}
