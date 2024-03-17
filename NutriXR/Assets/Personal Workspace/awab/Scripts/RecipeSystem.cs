
using System.Collections.Generic;
using System.Linq;
using OVRSimpleJSON;
using Personal_Workspace.joelk.DTOs;
using Personal_Workspace.joelk.Entities;
using Personal_Workspace.joelk.Scripts;
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
     private DataStorage dataStorage;
    [FormerlySerializedAs("shoppingCart")] [FormerlySerializedAs("basket")] [SerializeField] private BasketSystem basketSystem;
    [SerializeField]
    private GameObject recipeEntryPrefab;
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
                        }
                    }
                }
            }
        }
        for (var index = 0; index < _possibleRecipes.Count; index++)
        {
            var item = _possibleRecipes[index];
            GameObject recipeEntry = Instantiate(recipeEntryPrefab, recipeUIScrollViewContent.transform);
            recipeEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            var mAnchoredPosition = recipeEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x, y - (30 * index));
            /*GameObject itemPrefabInEntry = Instantiate(item.gameObject, recipeEntry.transform);
            itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
            itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
            itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefabInEntry.transform.localRotation = transform.parent.rotation;
            itemPrefabInEntry.SetActive(true);*/
        }
    }
}
