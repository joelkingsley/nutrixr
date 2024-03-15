
using System.Collections.Generic;
using System.Linq;
using OVRSimpleJSON;
using Personal_Workspace.joelk.DTOs;
using Personal_Workspace.joelk.Entities;
using Personal_Workspace.joelk.Scripts;
using TMPro;
using UnityEngine;

public class RecipeSystem : MonoBehaviour
{
    private List<IngredientItem> _selectedItems;
    [SerializeField]
    private GameObject recipeUIScrollViewContent;

    private List<IngredientItem> _possibleRecipes= new List<IngredientItem>();
    public List<RecipeItemData> allRecipes;
     private DataStorage dataStorage;
    [SerializeField] private Basket basket;
    [SerializeField]
    private GameObject recipeEntryPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _selectedItems = basket.selectedItems;
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

        _selectedItems = new List<IngredientItem>();
        foreach (var foodItem in _selectedItems)
        {
            //(Early vision for the code)
            //foreach recipe in _allRecipes:
            //  if fooditem.category in recipe.ingredients:
            //      _possibleRecipes.Add(recipe)
            foreach (var recipe in allRecipes)
            {
                foreach (var id in foodItem.data.categoryIds)
                {
                    if (recipe.categoryIdsWithWeights.Select(x => x.Item1).ToList().Contains<int>(id))
                    {
                        //_possibleRecipes.Add(RecipeItemData(recipe.inputIngredientCategories));
                    }
                }
            }
        }
        for (var index = 0; index < _possibleRecipes.Count; index++)
        {
            var item = _selectedItems[index];
            GameObject recipeEntry = Instantiate(recipeEntryPrefab, recipeUIScrollViewContent.transform);
            recipeEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.data.name;
            var mAnchoredPosition = recipeEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x, y - (30 * index));
            GameObject itemPrefabInEntry = Instantiate(item.gameObject, recipeEntry.transform);
            itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
            itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
            itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefabInEntry.transform.localRotation = transform.parent.rotation;
            itemPrefabInEntry.SetActive(true);
        }
    }
}
