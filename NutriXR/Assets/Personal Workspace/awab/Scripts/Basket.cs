using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi.Drawers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        selectedItems.Add(foodItem);
        Redraw();
    }

    private void Redraw()
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
            itemPrefabInEntry.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(itemPrefabInEntry.GetComponent<BoxCollider>());
            itemPrefabInEntry.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPrefabInEntry.transform.localScale = new Vector3(10, 10, 10);
            itemPrefabInEntry.transform.localPosition = new Vector3(0, 0, 0);
            itemPrefabInEntry.transform.localRotation = transform.parent.rotation;
            newBasketEntry.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                selectedItems.Remove(item);
                Redraw();
            });
            itemPrefabInEntry.SetActive(true);
        }
    }
}
