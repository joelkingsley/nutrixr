using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class RecipeCalculatedData
{
    public string RecipeName;
    public int RecipeId;
    public float Fat ;
    public float Sugar;
    public float Carbohydrates;
    public float Protein ;
    public float CaloriesInKcal ;
    public float Calories ;
    public char Nutriscorevalue;


    // Define positive and negative points and nutriscore
    float positivePoints = 0;
    float negativePoints = 0;
    float nutriScore= 0 ;

    public RecipeCalculatedData(RecipeItemData recipeItemData, List<IngredientItemData> ingredientItemDataList)
    {
         RecipeName = recipeItemData.name;
         RecipeId = recipeItemData.id;

        //tuple consist of ingredient id and weights


        foreach (var (ingredientId, ingredientWeight) in recipeItemData.categoryIdsWithWeights)
        {
            foreach (var ingredientItemData in ingredientItemDataList)
            {
                if (ingredientItemData.categoryIds.Any(id => id == ingredientId))
                {
                        Fat += ingredientItemData.fat * ingredientWeight;
                        Sugar += ingredientItemData.sugar * ingredientWeight;
                        Carbohydrates += ingredientItemData.carbohydrates * ingredientWeight;
                        Protein += ingredientItemData.protein * ingredientWeight;
                        Calories += ingredientItemData.calories * ingredientWeight;
                        CaloriesInKcal += ingredientItemData.caloriesInKcal * ingredientWeight;

                }
            }

            // Calculate negative points for high-quality nutrients
            if (Protein > 0 && Carbohydrates > 0)
            {
                // Assuming the Nutri-Score ranges from 0 to -5 for high-quality nutrients
                negativePoints -= 5; // Assigning maximum negative points for high-quality nutrients
            }

            // Calculate positive points for low-quality nutrients
            if (Fat > 0 || Sugar > 0 || Calories> 0 || CaloriesInKcal > 0)
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

}


