using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    public Image[] imagesPieChart;

    // Start is called before the first frame update
    void Start()
    {
        // Mock data
        Ingredient.FoodGroup[] foodGroups = new Ingredient.FoodGroup[]
        {
            Ingredient.FoodGroup.Butter,
            Ingredient.FoodGroup.Egg,
            Ingredient.FoodGroup.Meat,
            Ingredient.FoodGroup.FruitVegetable,
            Ingredient.FoodGroup.FruitVegetable,
            Ingredient.FoodGroup.FruitVegetable,
            Ingredient.FoodGroup.FruitVegetable,
            Ingredient.FoodGroup.FruitVegetable,
            Ingredient.FoodGroup.Milk,
            Ingredient.FoodGroup.Legume,
            Ingredient.FoodGroup.Potato

        };
        List<Ingredient> ingredients = new List<Ingredient>();
        foreach (var foodGroup in foodGroups)
        {
            Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
            ingredient.foodGroup = foodGroup;
            ingredients.Add(ingredient);
        }

        List<Recipe> recipes = new List<Recipe>();
        Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
        recipe.ingredients = ingredients.ToArray();
        recipe.weights = new float[]{1f,1f,1f,1f,1f,1f,1f,1f};
        recipes.Add(recipe);

        // ComputeValuesByCountingIngredients(ingredients);
        ComputeValuesByAddingIngredientWeights(recipes);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ComputeValuesByCountingIngredients(List<Ingredient> ingredients)
    {
        // Compute values
        var numberOfFruitVegetable = 0f;
        var numberOfLegumeNutSeed = 0f;
        var numberOfCerealsAndPotato = 0f;
        var numberOfOilAndFats = 0f;
        var numberOfMilkDiary = 0f;
        var numberOfMeatSausageFishEgg = 0f;
        var numberOfBeverages = 0f;
        foreach (var ingredient in ingredients)
        {
            switch (ingredient.foodGroup)
            {
                case Ingredient.FoodGroup.FruitVegetable:
                    numberOfFruitVegetable += 1;
                    break;
                case Ingredient.FoodGroup.Legume:
                case Ingredient.FoodGroup.NutSeed:
                    numberOfLegumeNutSeed += 1;
                    break;
                case Ingredient.FoodGroup.Potato:
                case Ingredient.FoodGroup.GrainBreadNoodle:
                    numberOfCerealsAndPotato += 1;
                    break;
                case Ingredient.FoodGroup.Butter:
                case Ingredient.FoodGroup.PlantOil:
                    numberOfOilAndFats += 1;
                    break;
                case Ingredient.FoodGroup.Milk:
                    numberOfMilkDiary += 1;
                    break;
                case Ingredient.FoodGroup.Egg:
                case Ingredient.FoodGroup.Fish:
                case Ingredient.FoodGroup.Meat:
                case Ingredient.FoodGroup.Sausage:
                    numberOfMeatSausageFishEgg += 1;
                    break;
                case Ingredient.FoodGroup.Juice:
                    numberOfBeverages += 1;
                    break;
            }
        }
        SetValues(new []{numberOfFruitVegetable, numberOfLegumeNutSeed, numberOfCerealsAndPotato, numberOfOilAndFats, numberOfMilkDiary, numberOfMeatSausageFishEgg});
    }

    void ComputeValuesByAddingIngredientWeights(List<Recipe> recipes)
    {
        // Compute values
        var fractionOfFruitVegetable = 0f;
        var fractionOfLegumeNutSeed = 0f;
        var fractionOfCerealsAndPotato = 0f;
        var fractionOfOilAndFats = 0f;
        var fractionOfMilkDiary = 0f;
        var fractionOfMeatSausageFishEgg = 0f;
        var fractionOfBeverages = 0f;
        foreach (var recipe in recipes)
        {
            foreach (var ingredientWithWeight in recipe.ingredients.Zip(recipe.weights, Tuple.Create))
            {
                switch (ingredientWithWeight.Item1.foodGroup)
                {
                    case Ingredient.FoodGroup.FruitVegetable:
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
                    case Ingredient.FoodGroup.Juice:
                        fractionOfBeverages += ingredientWithWeight.Item2;
                        break;
                }
            }
        }
        Debug.Log($"{fractionOfFruitVegetable}, {fractionOfLegumeNutSeed}, {fractionOfCerealsAndPotato}, {fractionOfOilAndFats}, {fractionOfMilkDiary}, {fractionOfMeatSausageFishEgg}");
        SetValues(new []{fractionOfFruitVegetable, fractionOfLegumeNutSeed, fractionOfCerealsAndPotato, fractionOfOilAndFats, fractionOfMilkDiary, fractionOfMeatSausageFishEgg});
    }

    public void SetValues(float[] valuesToSet)
    {
        float totalValues = 0;
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalValues += FindPercentage(valuesToSet, i);
            imagesPieChart[i].fillAmount = totalValues;
        }
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
