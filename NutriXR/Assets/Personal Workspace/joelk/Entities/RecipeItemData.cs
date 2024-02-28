using Personal_Workspace.joelk.DTOs;

namespace Personal_Workspace.joelk.Entities
{
    public class RecipeItemData: FoodItemData
    {
        public IngredientItemData[] IngredientItems;

        // public RecipeItemData(Recipe recipe, JsonReader.IngredientCategoryList ingredientCategoryList)
        // {
        //     name = recipe.name;
        //     foreach (var recipeInputIngredientCategory in recipe.inputIngredientCategories)
        //     {
        //         var currentCategory = ingredientCategoryList.ingredientCategories.Find(category =>
        //             category.categoryId == recipeInputIngredientCategory.id);
        //     }
        // }
    }
}
