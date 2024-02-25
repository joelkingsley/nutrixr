using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Basket : MonoBehaviour
{
    [SerializeField]
    private List<FoodItem> selectedItems;
    [SerializeField]
    private GameObject basketUIScrollViewContent;

    [SerializeField] private GameObject basketEntryPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToBasket(FoodItem foodItem)
    {
        GameObject newBasketEntry = Instantiate(basketEntryPrefab, basketUIScrollViewContent.transform);
        newBasketEntry.GetComponentInChildren<TextMeshProUGUI>().text = foodItem.data.name;
        var mAnchoredPosition = newBasketEntry.GetComponent<RectTransform>();
        var x = mAnchoredPosition.anchoredPosition.x;
        var y = mAnchoredPosition.anchoredPosition.y;
        mAnchoredPosition.anchoredPosition = new Vector2(x, y - (30*selectedItems.Count));
        GameObject itemPrefabInEntry = Instantiate(foodItem.gameObject, newBasketEntry.transform);
        itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
        itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
        itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
        itemPrefabInEntry.transform.localRotation= transform.parent.rotation;
        selectedItems.Add(foodItem);

    }
}
