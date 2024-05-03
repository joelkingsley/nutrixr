using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public RecipeSystem recipeSystem;
    public RecipeItemData recipeData;

    public void StartCooking()
    {
        recipeSystem.startCooking(recipeData);
    }
}
