using Personal_Workspace.joelk.DTOs;
using UnityEngine;
[System.Serializable]

public class FoodItemData
{
    public string name;
    public float calories;
    public float caloriesInKcal;
    public float carbohydrates;
    public float protein;
    public float fat;
    public float sugar;
    public bool isIngredient;

    public static FoodItemData CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<FoodItemData>(jsonString);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
