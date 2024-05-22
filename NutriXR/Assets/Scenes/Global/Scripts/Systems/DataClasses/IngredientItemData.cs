using System;
using Personal_Workspace.joelk.DTOs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    public string foodGroup;
    public char Nutriscorevalue;
    public int environmentScore;

    public IngredientItemData(IngredientDataObject ingredientDataObject)
    {
        //IngredientDataObject ingredientDataObject = JsonUtility.FromJson<IngredientDataObject>(jsonString);
        name = ingredientDataObject.name;
        fdcName = ingredientDataObject.fdcName;
        foodGroup = ingredientDataObject.foodGroup;

        Nutriscorevalue = ingredientDataObject.nutriScore.ToCharArray()[0];
    }

}


