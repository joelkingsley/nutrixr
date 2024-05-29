using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cooking : MonoBehaviour
{
    [FormerlySerializedAs("recipeSystem")] public BasketRecipeSystem basketRecipeSystem;
    public Recipe recipeData;

    public void StartCooking()
    {
        basketRecipeSystem.startCooking(recipeData);
    }
}
