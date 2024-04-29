
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasketSystem : MonoBehaviour
{
    public List<IngredientItem> selectedItems;
    [SerializeField]
    public GameObject basketUIScrollViewContent;
    [SerializeField]
    private RecipeSystem _recipeSystem;

    [SerializeField] private GameObject basketEntryPrefab;

    private TableItemSpawner _kitchenSceneTableItemSpawner;
    private TableItemSpawner _kitchenSceneFridgeItemSpawner;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
            _kitchenSceneTableItemSpawner = GameObject.FindGameObjectWithTag("KitchenTableIngredientSpawner").GetComponent<TableItemSpawner>();
            List<IngredientItemData> tableItemDatas = new List<IngredientItemData>();
            foreach (IngredientItem item in this.selectedItems)
            {
                if (item.spawnInFridge == false)
                {
                    tableItemDatas.Add(item.GetIngredientItemData());
                }
            }
            _kitchenSceneTableItemSpawner.SpawnKitchenSceneItems(tableItemDatas);

            _kitchenSceneFridgeItemSpawner = GameObject.FindGameObjectWithTag("KitchenFridgeIngredientSpawner").GetComponent<TableItemSpawner>();
            List<IngredientItemData> fridgeItemDatas = new List<IngredientItemData>();
            foreach (IngredientItem item in this.selectedItems)
            {
                if (item.spawnInFridge)
                {
                    fridgeItemDatas.Add(item.GetIngredientItemData());
                }
            }
            _kitchenSceneFridgeItemSpawner.SpawnKitchenSceneItems(fridgeItemDatas);
        }
    }

    public void AddToCart(IngredientItem ingredientItem)
    {
        Debug.Log("Add to cart");
        if (ingredientItem.GetIngredientItemData().name.Equals(""))
        {
            Debug.Log("Item without a name, wont be added to list");
            return;
        }

        selectedItems.Add(ingredientItem);
        //ingredientItem.transform.parent = shoppingCartGameObject.transform;
        if (basketUIScrollViewContent.activeSelf)
        {
            Redraw();
            _recipeSystem.RedrawRecipeUI();
        }

    }

    public void RemoveFromCart(IngredientItem ingredientItem)
    {
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

        for (var index = 0; index < selectedItems.Count; index++)
        {
            var item = selectedItems[index];
            GameObject newBasketEntry = Instantiate(basketEntryPrefab, basketUIScrollViewContent.transform);
            newBasketEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.GetIngredientItemData().name;
            var mAnchoredPosition = newBasketEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x+52, y - (30 * index)-15);

           /* GameObject itemPrefabInEntry = Instantiate(item.gameObject, newBasketEntry.transform);
            itemPrefabInEntry.GetComponent<Grabbable>().enabled = false;
            itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
            //Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
            itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
            itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefabInEntry.transform.localRotation = itemPrefabInEntry.transform.parent.rotation;*/

            newBasketEntry.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                item.RespawnToStart();
                RemoveFromCart(item);
            });
            //itemPrefabInEntry.SetActive(true);
        }
    }
}
