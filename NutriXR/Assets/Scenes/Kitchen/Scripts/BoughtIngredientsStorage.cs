using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoughtIngredientsStorage : MonoBehaviour
{
    private List<Ingredient> boughtIngredients = new List<Ingredient>();

    public void StoreAndSpawnBoughtIngredientList(List<Ingredient> ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            boughtIngredients.Add(ingredient);
        }

        //Un-cooled Ingredients
        List<Ingredient> tableItemDatas = new List<Ingredient>();
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.cooled == false)
            {
                tableItemDatas.Add(ingredient);
            }
        }
        GameObject.FindGameObjectWithTag("KitchenTableIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(tableItemDatas);

        //Cooled Ingredients
        List<Ingredient> fridgeItemDatas = new List<Ingredient>();
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.cooled)
            {
                fridgeItemDatas.Add(ingredient);
            }
        }
        GameObject.FindGameObjectWithTag("KitchenFridgeIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(fridgeItemDatas);
    }

    public List<Ingredient> GetIngredientList()
    {
        return boughtIngredients;
    }

}
