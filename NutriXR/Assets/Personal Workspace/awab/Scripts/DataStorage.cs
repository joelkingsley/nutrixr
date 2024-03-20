using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Personal_Workspace.joelk.DTOs;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    private List<IngredientDataObject> _ingredientDataObjects;

    private List<RecipeDataObject> _recipeDataObjects;
    [SerializeField] private TextAsset ingredientJsonFile;
    [SerializeField] private TextAsset recipeJsonFile;
    public Dictionary<string, IngredientItemData> IngredientItems = new();
    public Dictionary<int, RecipeItemData> RecipeItems = new();

    void Awake()
    {
        //fdcNames need to be hardcoded to prefabs
        _ingredientDataObjects = JsonUtility.FromJson<IngredientChoices>(ingredientJsonFile.text).ingredientChoices;
        foreach (var dataObject in _ingredientDataObjects)
        {
            IngredientItems.Add(dataObject.fdcName,new IngredientItemData(dataObject));
        }

        _recipeDataObjects = JsonUtility.FromJson<RecipeChoices>(recipeJsonFile.text).recipes;
        foreach (var dataObject in _recipeDataObjects)
        {
            RecipeItems.Add(dataObject.id,new RecipeItemData(dataObject));
        }
        Debug.Log(RecipeItems.Count);

        Debug.Log(IngredientItems.Count);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IngredientItemData ReadIngredientData(string fdcName)
    {
        return IngredientItems[fdcName];
    }

    public RecipeItemData ReadRecipeData(int recipeId)
    {
        return RecipeItems[recipeId];
    }

    public List<RecipeItemData>  ReadAllRecipes()
    {
        return RecipeItems.Values.ToList();
    }
}
