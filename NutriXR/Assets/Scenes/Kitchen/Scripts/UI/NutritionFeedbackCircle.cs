using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NutritionFeedbackCircle : MonoBehaviour
{
    [SerializeField]
    public GameObject feedbackCanvas;
    [SerializeField]
    public Image[] imagesPieChart;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Hide()
    {
        feedbackCanvas.SetActive(false);
    }

    public void Show()
    {
        feedbackCanvas.SetActive(true);
    }

    public void RenderConsumedRecipes(List<Recipe> consumedRecipes)
    {
        ComputeValuesByAddingIngredientWeights(consumedRecipes);
    }

    private void ComputeValuesByAddingIngredientWeights(List<Recipe> recipes)
    {
        // Compute values
        var fractionOfFruitVegetable = 0f;
        var fractionOfLegumeNutSeed = 0f;
        var fractionOfCerealsAndPotato = 0f;
        var fractionOfOilAndFats = 0f;
        var fractionOfMilkDiary = 0f;
        var fractionOfMeatSausageFishEgg = 0f;
        foreach (var recipe in recipes)
        {
            foreach (var ingredientWithWeight in recipe.ingredients.Zip(recipe.weights, Tuple.Create))
            {
                switch (ingredientWithWeight.Item1.foodGroup)
                {
                    case Ingredient.FoodGroup.FruitVegetable:
                    case Ingredient.FoodGroup.Juice:
                        fractionOfFruitVegetable += ingredientWithWeight.Item2;
                        break;
                    case Ingredient.FoodGroup.Legume:
                    case Ingredient.FoodGroup.NutSeed:
                        fractionOfLegumeNutSeed += ingredientWithWeight.Item2;
                        break;
                    case Ingredient.FoodGroup.Potato:
                    case Ingredient.FoodGroup.GrainBreadNoodle:
                        fractionOfCerealsAndPotato += ingredientWithWeight.Item2;
                        break;
                    case Ingredient.FoodGroup.Butter:
                    case Ingredient.FoodGroup.PlantOil:
                        fractionOfOilAndFats += ingredientWithWeight.Item2;
                        break;
                    case Ingredient.FoodGroup.Milk:
                        fractionOfMilkDiary += ingredientWithWeight.Item2;
                        break;
                    case Ingredient.FoodGroup.Egg:
                    case Ingredient.FoodGroup.Fish:
                    case Ingredient.FoodGroup.Meat:
                    case Ingredient.FoodGroup.Sausage:
                        fractionOfMeatSausageFishEgg += ingredientWithWeight.Item2;
                        break;
                }
            }
        }
        Debug.Log($"{fractionOfFruitVegetable}, {fractionOfLegumeNutSeed}, {fractionOfCerealsAndPotato}, {fractionOfOilAndFats}, {fractionOfMilkDiary}, {fractionOfMeatSausageFishEgg}");
        SetValues(new []{fractionOfFruitVegetable, fractionOfLegumeNutSeed, fractionOfCerealsAndPotato, fractionOfOilAndFats, fractionOfMilkDiary, fractionOfMeatSausageFishEgg});
    }

    public void SetValues(float[] valuesToSet)
    {
        DataLogger.Log("NutritionFeedbackCircle", "Start feedback:");
        string logEntry = "IngredientFractions";

        float totalValues = 0;
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalValues += FindPercentage(valuesToSet, i);
            logEntry += ";" + FindPercentage(valuesToSet, i);
            imagesPieChart[i].fillAmount = totalValues;
        }

        DataLogger.Log("NutritionFeedbackCircle", logEntry);
    }

    private float FindPercentage(float[] valuesToSet, int index)
    {
        float totalAmount = 0;
        foreach (var value in valuesToSet)
        {
            totalAmount += value;
        }

        return valuesToSet[index] / totalAmount;
    }
}
