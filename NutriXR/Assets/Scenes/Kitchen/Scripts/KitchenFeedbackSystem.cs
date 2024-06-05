using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenFeedbackSystem : MonoBehaviour
{
    [SerializeField] private NutritionFeedbackCircle nutritionFeedbackCircle;
    [SerializeField] private EnvironmentFeedbackBars environmentFeedbackBars;

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

        //StartFeedback("Nutrition");
        StartFeedback("NotNutritionBecauseItOnlyComparesForNutritionSoThisShouldStillWork");
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
            environmentFeedbackBars.Hide();
            nutritionFeedbackCircle.Show();
        }
        else
        {
            environmentFeedbackBars.RenderConsumedRecipes(consumedRecipes);
            nutritionFeedbackCircle.Hide();
            environmentFeedbackBars.Show();
        }
    }
}
