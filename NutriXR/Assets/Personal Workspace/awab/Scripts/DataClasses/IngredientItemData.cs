using Personal_Workspace.joelk.DTOs;
using UnityEngine;
[System.Serializable]

public class IngredientItemData
{
    public string name;
    public float calories;
    public float caloriesInKcal;
    public float carbohydrates;
    public float protein;
    public float fat;
    public float sugar;
    public int[] categoryIds;
    public string fdcName;

        public IngredientItemData(IngredientDataObject ingredientDataObject )
        {
            //IngredientDataObject ingredientDataObject = JsonUtility.FromJson<IngredientDataObject>(jsonString);
            name = ingredientDataObject.name;
            fdcName = ingredientDataObject.fdcName;
            var fatIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Total fat (NLEA)" | nutrient.nutrient.name == "Total lipid (fat)");
            if (fatIndex >= 0)
            {
                fat = ingredientDataObject.data.foodNutrients[fatIndex].amount;
            }
            var sugarIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                nutrient => nutrient.nutrient.name == "Sugars, total including NLEA" | nutrient.nutrient.name == "Sugars, Total");
            if (sugarIndex >= 0)
            {
                sugar = ingredientDataObject.data.foodNutrients[sugarIndex].amount;
            }
            var carbohydratesIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                    nutrient => nutrient.nutrient.name == "Carbohydrate, by difference");
            if (carbohydratesIndex >= 0)
            {
                carbohydrates = ingredientDataObject.data.foodNutrients[carbohydratesIndex].amount;
            }
            var proteinIndex = ingredientDataObject.data.foodNutrients.FindIndex(
                nutrient => nutrient.nutrient.name == "Protein");
            if (proteinIndex >= 0)
            {
                protein = ingredientDataObject.data.foodNutrients[proteinIndex].amount;
            }
            var caloriesIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kJ");
            if (caloriesIndex >= 0)
            {
                calories = ingredientDataObject.data.foodNutrients[caloriesIndex].amount;
            }
            var caloriesInKcalIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
                nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kcal");
            if (caloriesInKcalIndex >= 0)
            {
                caloriesInKcal = ingredientDataObject.data.foodNutrients[caloriesInKcalIndex].amount;
            }

            categoryIds = ingredientDataObject.categoryIds;
        }

    /*public static FoodItemData CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<FoodItemData>(jsonString);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }*/
}
