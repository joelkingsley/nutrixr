using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedRecipes : MonoBehaviour
{
    private List<Recipe> recipes = new List<Recipe>();

    public void addRecipe(Recipe recipe)
    {
        recipes.Add(recipe);
    }
}
