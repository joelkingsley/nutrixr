using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeItemData
{
    public string name;
    public int id;
    public List<(int, float)> categoryIdsWithWeights;

    public RecipeItemData(RecipeDataObject recipeDataObject)
    {
        name = recipeDataObject.name;
        id = recipeDataObject.id;
        categoryIdsWithWeights = recipeDataObject.inputIngredientCategories.Select(x => (x.id, x.ingredientWeight)).ToList();
    }
}
