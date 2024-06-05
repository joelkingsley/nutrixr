using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenFeedbackSystem : MonoBehaviour
{
    [SerializeField]
    public NutritionFeedbackCircle nutritionFeedbackCircle;

    private List<Recipe> consumedRecipes;

    // Start is called before the first frame update
    void Start()
    {
        consumedRecipes = new List<Recipe>();

        Debug();
    }

    void Debug()
    {
        List<Recipe> loadedRecipeList = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes/ScriptableObjects"));
        consumedRecipes = loadedRecipeList;

        StartFeedback("Nutrition");
    }

    public void TrackConsumedRecipe(Recipe consumedRecipe)
    {
        consumedRecipes.Add(consumedRecipe);
    }

    public void StartFeedback(string feedbackMode)
    {
        if (feedbackMode.Equals("Nutrition"))
        {
            nutritionFeedbackCircle.RenderConsumedRecipes(consumedRecipes);
            nutritionFeedbackCircle.Show();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
