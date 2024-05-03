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
    public char Nutriscorevalue;

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

        //### Nutriscore calculation###
        // Define positive and negative points and nutriscore
        float positivePoints = 0;
        float negativePoints = 0;
        float nutriScore= 0 ;

        // Calculate negative points for high-quality nutrients
        if (protein > 0 && carbohydrates > 0)
        {
            // Assuming the Nutri-Score ranges from 0 to -5 for high-quality nutrients
            negativePoints -= 5; // Assigning maximum negative points for high-quality nutrients
        }


        // Calculate positive points for low-quality nutrients
        if (fat > 0 || sugar > 0 || calories> 0 || caloriesInKcal > 0)
        {
            // Assuming the Nutri-Score ranges from 0 to 10 for low-quality nutrients
            positivePoints += 10; // Assigning maximum positive points for low-quality nutrients
        }

        // Calculate total Nutri score
        nutriScore = negativePoints - positivePoints;

        // Assign letter grade based on Nutri score
        if (nutriScore <= -15)
        {
            Nutriscorevalue = 'A';
        }
        else if (nutriScore <= -10)
        {
            Nutriscorevalue =  'B';
        }
        else if (nutriScore <= -5)
        {
            Nutriscorevalue = 'C';
        }
        else if (nutriScore <= 0)
        {
            Nutriscorevalue ='D';
        }
        else
        {
            Nutriscorevalue='E';
        }
    }
}
