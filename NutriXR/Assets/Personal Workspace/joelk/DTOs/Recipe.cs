using System;
using System.Collections.Generic;

namespace Personal_Workspace.joelk.DTOs
{
    [Serializable]
    public class Recipe
    {
        public string id;
        public string name;

        public List<int> ingredientCategories;
    }
}
