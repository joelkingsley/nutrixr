using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class RecipeNutritionData : MonoBehaviour
{
    public string recipeName;
    public int recipeId;
    public List<(int, float)> categoryIdsWithWeights;

    public float calories;
    public float caloriesInKcal;
    public float carbohydrates;
    public float protein;
    public float fat;
    public float sugar;


    public RecipeNutritionData(RecipeDataObject recipeDataObject, IngredientDataObject ingredientDataObject)
    {
        recipeName = recipeDataObject.name;
        recipeId = recipeDataObject.id;
        //tuple consist of ingredient id and weights
        categoryIdsWithWeights = recipeDataObject.inputIngredientCategories.Select(x => (x.id, x.ingredientWeight)).ToList();


        foreach (var (ingredientId, ingredientWeight) in categoryIdsWithWeights)
        {
            if (ingredientDataObject.categoryIds.Any(id => id == ingredientId))
            {
                var fatIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Total fat (NLEA)" |
                                nutrient.nutrient.name == "Total lipid (fat)");
                if (fatIndex >= 0)
                {
                    fat += ingredientDataObject.data.foodNutrients[fatIndex].amount * ingredientWeight;
                }

                var sugarIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Sugars, total including NLEA" |
                                nutrient.nutrient.name == "Sugars, Total");
                if (sugarIndex >= 0)
                {
                    sugar += ingredientDataObject.data.foodNutrients[sugarIndex].amount * ingredientWeight;
                }

                var carbohydratesIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Carbohydrate, by difference");
                if (carbohydratesIndex >= 0)
                {
                    carbohydrates += ingredientDataObject.data.foodNutrients[carbohydratesIndex].amount * ingredientWeight;
                }

                var proteinIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Protein");
                if (proteinIndex >= 0)
                {
                    protein += ingredientDataObject.data.foodNutrients[proteinIndex].amount * ingredientWeight;
                }

                var caloriesIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
                    nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kJ");
                if (caloriesIndex >= 0)
                {
                    calories += ingredientDataObject.data.foodNutrients[caloriesIndex].amount * ingredientWeight;
                }

                var caloriesInKcalIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
                    nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kcal");
                if (caloriesInKcalIndex >= 0)
                {
                    caloriesInKcal += ingredientDataObject.data.foodNutrients[caloriesInKcalIndex].amount * ingredientWeight;
                }
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
