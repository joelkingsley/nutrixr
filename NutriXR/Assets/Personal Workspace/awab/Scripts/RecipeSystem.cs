
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeSystem : MonoBehaviour
{
    private List<FoodItem> _selectedItems;
    [SerializeField]
    private GameObject recipeUIScrollViewContent;

    private List<FoodItem> _possibleRecipes= new List<FoodItem>();

    [SerializeField]
    private GameObject recipeEntryPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _selectedItems = GetComponent<Basket>().selectedItems;

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
        /*
         *
         * TODO: Calculate the _possibleRecipes here
         *
         */
        _selectedItems = new List<FoodItem>();
        foreach (var foodItem in _selectedItems)
        {
            //(Early vision for the code)
            //foreach recipe in _allRecipes:
            //  if fooditem.category in recipe.ingredients:
            //      _possibleRecipes.Add(recipe)
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
