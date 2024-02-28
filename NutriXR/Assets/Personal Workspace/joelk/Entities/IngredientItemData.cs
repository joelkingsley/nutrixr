using System;
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
            fat = ingredientChoice.data.foodNutrients.Find(nutrient => nutrient.nutrient.name == "Total fat (NLEA)")
                .amount;
            sugar = ingredientChoice.data.foodNutrients.Find(
                nutrient => nutrient.nutrient.name == "Sugars, total including NLEA" | nutrient.nutrient.name ==
                    "Sugars, Total").amount;
            carbohydrates = ingredientChoice.data.foodNutrients.Find(
                    nutrient => nutrient.nutrient.name == "Carbohydrate, by difference").amount;
            protein = ingredientChoice.data.foodNutrients.Find(
                nutrient => nutrient.nutrient.name == "Protein").amount;
            calories = ingredientChoice.data.foodNutrients.Find(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kJ").amount;
            caloriesInKcal = ingredientChoice.data.foodNutrients.Find(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kcal").amount;
            isIngredient = true;
        }
    }
}
