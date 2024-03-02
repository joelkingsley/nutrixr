using UnityEngine;
[System.Serializable]

public class FoodItemData
{
    public string name;
    public int calories;
    public int carbohydrates;
    public int protein;
    public int fat;
    public int sugar;
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
