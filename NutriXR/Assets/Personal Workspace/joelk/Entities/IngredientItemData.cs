using System;
using System.Linq;
using Personal_Workspace.joelk.DTOs;
using Unity.VisualScripting;

namespace Personal_Workspace.joelk.Entities
{
    [Serializable]
    public class IngredientItemData: FoodItemData
    {
        public string fdcName;

        public IngredientItemData(IngredientChoice ingredientChoice)
        {
            name = ingredientChoice.name;
            fdcName = ingredientChoice.fdcName;
            var fatIndex = ingredientChoice.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Total fat (NLEA)" | nutrient.nutrient.name == "Total lipid (fat)");
            if (fatIndex >= 0)
            {
                fat = ingredientChoice.data.foodNutrients[fatIndex].amount;
            }
            var sugarIndex = ingredientChoice.data.foodNutrients.FindIndex(
                nutrient => nutrient.nutrient.name == "Sugars, total including NLEA" | nutrient.nutrient.name == "Sugars, Total");
            if (sugarIndex >= 0)
            {
                sugar = ingredientChoice.data.foodNutrients[sugarIndex].amount;
            }
            var carbohydratesIndex = ingredientChoice.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Carbohydrate, by difference");
            if (carbohydratesIndex >= 0)
            {
                carbohydrates = ingredientChoice.data.foodNutrients[carbohydratesIndex].amount;
            }
            var proteinIndex = ingredientChoice.data.foodNutrients.FindIndex(
                nutrient => nutrient.nutrient.name == "Protein");
            if (proteinIndex >= 0)
            {
                protein = ingredientChoice.data.foodNutrients[proteinIndex].amount;
            }
            var caloriesIndex = ingredientChoice.data.foodNutrients.FindIndex(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kJ");
            if (caloriesIndex >= 0)
            {
                calories = ingredientChoice.data.foodNutrients[caloriesIndex].amount;
            }
            var caloriesInKcalIndex = ingredientChoice.data.foodNutrients.FindIndex(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kcal");
            if (caloriesInKcalIndex >= 0)
            {
                caloriesInKcal = ingredientChoice.data.foodNutrients[caloriesInKcalIndex].amount;
            }
            isIngredient = true;
        }
    }
}
