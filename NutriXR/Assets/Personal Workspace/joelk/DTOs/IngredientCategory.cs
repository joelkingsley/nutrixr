using System.Collections.Generic;

namespace Personal_Workspace.joelk.DTOs
{
    public class IngredientCategory
    {
        public string CategoryId;
        public string Name;
        public List<string> FdcNamesOfIngredientChoices;
        public List<int> IdsOfRecipesThatHaveThisCategory;
    }
}
