using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasketSystem : MonoBehaviour
{
    [SerializeField] public GameObject basketUIScrollViewContent;
    [SerializeField] private RecipeSystem _recipeSystem;
    [SerializeField] private GameObject basketEntryPrefab;

    public List<IngredientItem> selectedItems;

    // Start is called before the first frame update
    void Start()
    {
        selectedItems = new List<IngredientItem>();
        Debug.Log("Size of List: " + selectedItems.Count);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if (scene.name == "Kitchen")
        {
            //Un-cooled Ingredients
            List<Ingredient> tableItemDatas = new List<Ingredient>();
            foreach (IngredientItem item in selectedItems)
            {
                if (item.ingredient.cooled == false)
                {
                    tableItemDatas.Add(item.ingredient);
                }
            }
            GameObject.FindGameObjectWithTag("KitchenTableIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(tableItemDatas);

            //Cooled Ingredients
            List<Ingredient> fridgeItemDatas = new List<Ingredient>();
            foreach (IngredientItem item in selectedItems)
            {
                if (item.ingredient.cooled)
                {
                    fridgeItemDatas.Add(item.ingredient);
                }
            }
            GameObject.FindGameObjectWithTag("KitchenFridgeIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(fridgeItemDatas);
        }
    }

    public void AddToCart(IngredientItem ingredientItem)
    {
        if (ingredientItem == null)
        {
            Debug.LogError("Tried to add a null Ingredient to shopping cart");
            return;
        }
        //TODO NullPointer
        Debug.Log("Add to cart");
        if (ingredientItem.ingredient.name.Equals(""))
        {
            Debug.Log("Item without a name, wont be added to list");
            return;
        }
        Debug.Log("Size of List before Add: " + selectedItems.Count);
        selectedItems.Add(ingredientItem);
        Debug.Log("Size of List after Add: " + selectedItems.Count);
        //ingredientItem.transform.parent = shoppingCartGameObject.transform;
        if (basketUIScrollViewContent.activeSelf)
        {
            Redraw();
            _recipeSystem.RedrawRecipeUI();
        }

    }

    public void RemoveFromCart(IngredientItem ingredientItem)
    {
        if (ingredientItem == null)
        {
            Debug.LogError("Tried to remove a null Ingredient from shopping cart");
            return;
        }
        selectedItems.Remove(ingredientItem);
        _recipeSystem.RedrawRecipeUI();
        Redraw();
    }

    public void Redraw()
    {
        foreach(Transform child in basketUIScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (IngredientItem item in selectedItems)
        {
            GameObject newBasketEntry = Instantiate(basketEntryPrefab, basketUIScrollViewContent.transform);
            newBasketEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.ingredient.name;
            var mAnchoredPosition = newBasketEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x+52, y - (30 * index)-15);

            newBasketEntry.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                //TODO
            });
            index++;
        }
    }
}
