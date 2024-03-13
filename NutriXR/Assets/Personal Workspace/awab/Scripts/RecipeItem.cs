using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : MonoBehaviour
{
    public int recipeId;
    public RecipeItemData data;
    private DataStorage dataStorage;
    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadRecipeData(recipeId);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
