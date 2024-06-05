using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Avatar2;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentFeedbackBars : MonoBehaviour
{

    [SerializeField] private GameObject EnvironmentCanvas;
    [SerializeField] private Image CO2;
    [SerializeField] private Image Water;
    [SerializeField] private Image Land;

    //Attention: These values are guessed!
    [SerializeField] private float SustainableCO2 = 5;
    [SerializeField] private float SustainableWater = 100;
    [SerializeField] private float SustainableLand = 500;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RenderConsumedRecipes(List<Recipe> consumedRecipes)
    {
        // Compute values
        float co2Emission = 0;
        float landUse = 0;
        float waterUse = 0;

        foreach (Recipe recipe in consumedRecipes)
        {
            foreach (var ingredientWithWeight in recipe.ingredients.Zip(recipe.weights, Tuple.Create))
            {
                //CO2Emission is normed to 100g. Weight is gramm ofr Ingredient
                float normedWeight = (ingredientWithWeight.Item2 / 100);
                co2Emission += ingredientWithWeight.Item1.CO2Emission * normedWeight;
                landUse += ingredientWithWeight.Item1.LandUse * normedWeight;
                waterUse += ingredientWithWeight.Item1.WaterUse * normedWeight;
            }
        }
        //Debug.Log("CO2: " + co2Emission);
        //Debug.Log("Land: " + landUse);
        //Debug.Log("Water: " + waterUse);

        float co2fill = ((co2Emission / SustainableCO2) / 2) + 0.01f;
        float waterfill = ((waterUse / SustainableWater)) / 2 + 0.01f;
        float landfill = ((landUse / SustainableLand) / 2) + 0.01f;

        CO2.fillAmount = co2fill;
        if (co2fill < 0.4)
        {
            CO2.color = Color.green;
        } else if (co2fill >= 0.4 && co2fill <= 0.6)
        {
            CO2.color = Color.yellow;
        }
        else
        {
            CO2.color = Color.red;
        }

        Water.fillAmount = waterfill;
        if (waterfill < 0.4)
        {
            Water.color = Color.green;
        } else if (waterfill >= 0.4 && waterfill <= 0.6)
        {
            Water.color = Color.yellow;
        }
        else
        {
            Water.color = Color.red;
        }

        Land.fillAmount = landfill;
        if (landfill < 0.4)
        {
            Land.color = Color.green;
        } else if (landfill >= 0.4 && landfill <= 0.6)
        {
            Land.color = Color.yellow;
        }
        else
        {
            Land.color = Color.red;
        }
    }

    public void Show()
    {
        EnvironmentCanvas.SetActive(true);
    }

    public void Hide()
    {
        EnvironmentCanvas.SetActive(false);
    }
}
