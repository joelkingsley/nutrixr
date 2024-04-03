
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeSystem : MonoBehaviour
{
    private List<IngredientItem> _selectedItems;
    [SerializeField]
    private GameObject recipeUIScrollViewContent;

    private List<RecipeItemData> _possibleRecipes = new ();
    public List<RecipeItemData> allRecipes;
    public List<List<int>> categoriesInPossibleRecipes = new();
     private DataStorage dataStorage;
    [FormerlySerializedAs("shoppingCart")] [FormerlySerializedAs("basket")] [SerializeField] private BasketSystem basketSystem;
    [SerializeField]
    private GameObject recipeEntryPrefab;
    [SerializeField]
    private GameObject categoryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _selectedItems = basketSystem.selectedItems;
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        allRecipes = dataStorage.ReadAllRecipes();
    }

    // Update is called once per frame
    void Update()
    {

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
        foreach (var foodItem in _selectedItems)
        {
            foreach (var recipe in allRecipes)
            {
                foreach (var id in foodItem.GetIngredientItemData().categoryIds)
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
        for (var index = 0; index < _possibleRecipes.Count; index++)
        {
            var item = _possibleRecipes[index];
            GameObject recipeEntry = Instantiate(recipeEntryPrefab, recipeUIScrollViewContent.transform);
            recipeEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            var mAnchoredPosition = recipeEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x+52, y - (30 * index + 10*numberOfAddedCategories)-15);
            for (var j = 0; j < categoriesInPossibleRecipes[index].Count; j++)
            {
                var categoryId = categoriesInPossibleRecipes[index][j];
                GameObject newCategoryTextGameObject = Instantiate(categoryPrefab, recipeEntry.transform);
                 newCategoryTextGameObject.AddComponent<TextMeshProUGUI>();

                var categoryPos = newCategoryTextGameObject.GetComponent<RectTransform>();
                var categoryX = categoryPos.anchoredPosition.x;
                var categoryY = categoryPos.anchoredPosition.y;
                categoryPos.anchoredPosition = new Vector2(categoryX+10, categoryY - (10 * j));
                TextMeshProUGUI textComponent = newCategoryTextGameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = dataStorage.GetCategoryName(categoryId);
                foreach (var cSelectedItem in basketSystem.selectedItems)
                {
                    if (cSelectedItem.GetIngredientItemData().categoryIds.Contains(categoryId))
                    {
                        textComponent.color = Color.green;
                    }
                }
                numberOfAddedCategories += 1; //this is used to adjust the offset of the elements
            }
        }
    }
}
