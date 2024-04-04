using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeItem : MonoBehaviour
{
    public int recipeId;
    private RecipeItemData data;
    public RecipeCalculatedData CalculatedData;
    private DataStorage dataStorage;
    public BasketSystem basketSystem;

    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadRecipeData(recipeId);
        var ingredientItemDataList = basketSystem.selectedItems.Select(x => x.GetIngredientItemData()).ToList();
        CalculatedData = new RecipeCalculatedData(data, ingredientItemDataList);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
