using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenFeedbackSystem : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private NutritionFeedbackCircle nutritionFeedbackCircle;
    [SerializeField] private EnvironmentFeedbackBars environmentFeedbackBars;
    public bool calculatedFeedback = false;

    private List<Recipe> consumedRecipes;

    // Start is called before the first frame update
    void Start()
    {
        consumedRecipes = new List<Recipe>();
        Canvas.SetActive(false);
    }

    public void TrackConsumedRecipe(Recipe consumedRecipe)
    {
        consumedRecipes.Add(consumedRecipe);
    }

    public void StartFeedback()
    {
        if (calculatedFeedback || consumedRecipes.Count == 0) return;
        calculatedFeedback = true;
        GetComponent<AudioSource>().Play();
        string feedbackMode = DataLogger.GOAL;
        Canvas.SetActive(true);

        DataLogger.Log("KitchenFeedbackSystem", "Consumed Recipes:");
        foreach (Recipe recipe in consumedRecipes)
        {
            DataLogger.Log("KitchenFeedbackSystem", recipe.name);
        }

        if (feedbackMode.Equals("Nutrition"))
        {
            nutritionFeedbackCircle.RenderConsumedRecipes(consumedRecipes);
            environmentFeedbackBars.Hide();
            nutritionFeedbackCircle.Show();
            DataLogger.Log("KitchenFeedbackSystem", "Showing Nutrition Circle...");
        }
        else
        {
            environmentFeedbackBars.RenderConsumedRecipes(consumedRecipes);
            nutritionFeedbackCircle.Hide();
            environmentFeedbackBars.Show();
            DataLogger.Log("KitchenFeedbackSystem", "Showing Environment Bars...");
        }
    }
}
