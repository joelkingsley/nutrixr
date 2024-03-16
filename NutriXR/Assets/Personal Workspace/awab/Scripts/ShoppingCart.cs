
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCart : MonoBehaviour
{
    public List<IngredientItem> selectedItems;
    [SerializeField]
    private GameObject basketUIScrollViewContent;
    [SerializeField]
    private RecipeSystem _recipeSystem;
    private GameObject shoppingCartGameObject;

    [SerializeField] private GameObject basketEntryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        shoppingCartGameObject = GameObject.FindGameObjectWithTag("ShoppingCart");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToCart(IngredientItem ingredientItem)
    {
        selectedItems.Add(ingredientItem);
        //ingredientItem.transform.parent = shoppingCartGameObject.transform;
        if (basketUIScrollViewContent.activeSelf)
        {
            Redraw();
            _recipeSystem.RedrawRecipeUI();
        }

    }

    public void RemoveFromCart(IngredientItem item)
    {
        selectedItems.Remove(item);
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
            newBasketEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.data.name;
            var mAnchoredPosition = newBasketEntry.GetComponent<RectTransform>();
            var x = mAnchoredPosition.anchoredPosition.x;
            var y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x, y - (30 * index));

            GameObject itemPrefabInEntry = Instantiate(item.gameObject, newBasketEntry.transform);
            itemPrefabInEntry.GetComponent<Grabbable>().enabled = false;


            itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
            //Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
            itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
            itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefabInEntry.transform.localRotation = itemPrefabInEntry.transform.parent.rotation;
            newBasketEntry.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                item.RespawnToStart();
                RemoveFromCart(item);
            });
            itemPrefabInEntry.SetActive(true);
        }
    }
}
