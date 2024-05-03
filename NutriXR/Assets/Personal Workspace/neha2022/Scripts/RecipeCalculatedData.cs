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


        }

    }

}


