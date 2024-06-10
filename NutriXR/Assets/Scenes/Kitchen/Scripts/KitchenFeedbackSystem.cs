using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenFeedbackSystem : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private NutritionFeedbackCircle nutritionFeedbackCircle;
    [SerializeField] private EnvironmentFeedbackBars environmentFeedbackBars;

    private List<Recipe> consumedRecipes;

    private bool temp = false;

    // Start is called before the first frame update
    void Start()
    {
        consumedRecipes = new List<Recipe>();
        Canvas.SetActive(false);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) && temp)
        {
            StartFeedback("Nutrition");
            temp = false;
        } else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) && !temp)
        {
            StartFeedback("sdgsdg");
            temp = true;
        }
    }

    public void TrackConsumedRecipe(Recipe consumedRecipe)
    {
        consumedRecipes.Add(consumedRecipe);
    }

    public void StartFeedback(string feedbackMode)
    {
        Canvas.SetActive(true);
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
