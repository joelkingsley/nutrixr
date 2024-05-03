
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeSystem : MonoBehaviour
{
    private List<IngredientItem> _selectedItems;
    [SerializeField]
    public GameObject recipeUIScrollViewContent;

    private List<RecipeItemData> _possibleRecipes = new();
    public List<RecipeItemData> allRecipes;
    public List<List<int>> categoriesInPossibleRecipes = new();
     private DataStorage dataStorage;
    [FormerlySerializedAs("shoppingCart")] [FormerlySerializedAs("basket")] [SerializeField] private BasketSystem basketSystem;
    [SerializeField]
    private GameObject recipeEntryPrefab;
    [SerializeField]
    private GameObject categoryPrefab;

    [SerializeField] private TableItemSpawner recipeSpawner;
    private List<GameObject> cookedRecipes = new();

    void Start()
    {
        _selectedItems = basketSystem.selectedItems;
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        allRecipes = dataStorage.ReadAllRecipes();
    }

    public void RedrawRecipeUI()
    {
        //clear the UI
        foreach(Transform child in recipeUIScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        _possibleRecipes = new List<RecipeItemData>();
        categoriesInPossibleRecipes = new List<List<int>>();
        foreach (IngredientItem foodItem in _selectedItems)
        {
            foreach (RecipeItemData recipe in allRecipes)
            {
                foreach (int id in foodItem.GetIngredientItemData().categoryIds)
                {
                    if (recipe.categoryIdsWithWeights.Select(x => x.Item1).ToList().Contains<int>(id))
                    {
                        if (!_possibleRecipes.Contains(recipe))
                        {
                            _possibleRecipes.Add(recipe);
                            categoriesInPossibleRecipes.Add(recipe.categoryIdsWithWeights.Select(x=>x.Item1).ToList());
                        }
                    }
                }
            }
        }

        int numberOfAddedCategories = 0;//this is used to adjust the offset of the elements
        for (int index = 0; index < _possibleRecipes.Count; index++)
        {
            bool recipeFinished = true;
            RecipeItemData item = _possibleRecipes[index];
            GameObject recipeEntry = Instantiate(recipeEntryPrefab, recipeUIScrollViewContent.transform);
            recipeEntry.transform.GetChild(1).GetComponent<Cooking>().recipeSystem = this;
            recipeEntry.transform.GetChild(1).GetComponent<Cooking>().recipeData = item;
            recipeEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            RectTransform mAnchoredPosition = recipeEntry.GetComponent<RectTransform>();
            float x = mAnchoredPosition.anchoredPosition.x;
            float y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x+52, y - (30 * index + 10*numberOfAddedCategories)-15);
            for (int j = 0; j < categoriesInPossibleRecipes[index].Count; j++)
            {
                int categoryId = categoriesInPossibleRecipes[index][j];
                GameObject newCategoryTextGameObject = Instantiate(categoryPrefab, recipeEntry.transform);
                newCategoryTextGameObject.AddComponent<TextMeshProUGUI>();

                RectTransform categoryPos = newCategoryTextGameObject.GetComponent<RectTransform>();
                float categoryX = categoryPos.anchoredPosition.x;
                float categoryY = categoryPos.anchoredPosition.y;
                categoryPos.anchoredPosition = new Vector2(categoryX+10, categoryY - (10 * j));
                TextMeshProUGUI textComponent = newCategoryTextGameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = dataStorage.GetCategoryName(categoryId);
                bool foundIngrediendCategory = false;
                foreach (IngredientItem cSelectedItem in basketSystem.selectedItems)
                {
                    if (cSelectedItem.GetIngredientItemData().categoryIds.Contains(categoryId))
                    {
                        textComponent.color = Color.green;
                        foundIngrediendCategory = true;
                    }
                }

                if (!foundIngrediendCategory) recipeFinished = false;
                numberOfAddedCategories += 1; //this is used to adjust the offset of the elements
            }

            if (recipeFinished)
            {
                recipeEntry.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                recipeEntry.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void startCooking(RecipeItemData recipeData)
    {
        //Debug.Log("Cooking: " + recipeData.name);
        GameObject prefab = (GameObject) Resources.Load("RecipePrefabs/" + recipeData.name, typeof(GameObject));
        GameObject recipeObject = Instantiate(prefab);
        cookedRecipes.Add(recipeObject);
        recipeSpawner.addItem(recipeObject);

        foreach (IngredientItem selectedItem in basketSystem.selectedItems)
        {
            List<int> categoryIDs = recipeData.categoryIdsWithWeights.Select(x => x.Item1).ToList();
            foreach (int categoryId in categoryIDs)
            {
                if (selectedItem.GetIngredientItemData().categoryIds.Contains(categoryId))
                {
                    recipeObject.GetComponent<RecipeIngredients>().ingredients.Add(selectedItem.fdcName);
                    basketSystem.RemoveFromCart(selectedItem);
                    Destroy(selectedItem.gameObject);
                    return;
                }
            }
        }
    }
}
