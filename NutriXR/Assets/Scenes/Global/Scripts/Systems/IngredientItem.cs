using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngredientItem : MonoBehaviour
{
    private IngredientItemData data;
    private BasketSystem basketSystem;
    private DataStorage dataStorage;
    private GameObject shoppingCart;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;

    private Collider[] allColliders;
    private float inPotScaleModifier = 0.5f;

    [SerializeField]
    public string fdcName;

    [SerializeField]
    public bool spawnInFridge;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Supermarket"))
        {
            basketSystem = GameObject.FindGameObjectWithTag("BasketSystem").GetComponent<BasketSystem>();
        }
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);

        startingPosition = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;

        allColliders = GetComponentsInChildren<Collider>(false);
    }

    private void ChangeAllLayers(string newLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(newLayer);
        foreach (Collider c in allColliders)
        {
            c.gameObject.layer = LayerMask.NameToLayer(newLayer);
        }
    }

    public void OnSelect()
    {
        ChangeAllLayers("SelectedIngredientItem");

        //transform.parent = null;
        //shoppingCart.GetComponentInParent<CartSync>().RemoveItemFromCart(this);
        //basketSystem.RemoveFromCart(this);
    }

    public void OnUnselect()
    {
        ChangeAllLayers("PendingIngredientItem");
    }

    private void OnTriggerEnter(Collider other)
    {
        //UnselectedIngredientItem: No behaviour
        //SelectedIngredientItem: No behaviour
        //ShoppingCart: No behaviour

        if (shoppingCart == null)
        {
            shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartItemHook");
        }

        if (gameObject.layer == LayerMask.NameToLayer("PendingIngredientItem")
            && other.gameObject.layer == LayerMask.NameToLayer("ShoppingCart"))
        {
            //PendingIngredientItem <-> ShoppingCart: The pending item becomes part of shopping cart
            transform.SetParent(shoppingCart.transform, true);
            shoppingCart.GetComponentInParent<CartSync>().AddItemToCart(this);
            basketSystem.AddToCart(this);
            ChangeAllLayers("ShoppingCart");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("PendingIngredientItem")
            && other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            //PendingIngredientItem <-> Default: The pending item is dropped and thus unselected
            ChangeAllLayers("UnselectedIngredientItem");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (shoppingCart == null)
        {
            shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartItemHook");
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("ShoppingCart"))
        {
            transform.parent = null;
            shoppingCart.GetComponentInParent<CartSync>().RemoveItemFromCart(this);
            basketSystem.RemoveFromCart(this);
            if (gameObject.layer != LayerMask.NameToLayer("SelectedIngredientItem"))
            {
                ChangeAllLayers("PendingIngredientItem");
            }
        }
    }

    public void RespawnToStart()
    {
        //gameObject.transform.position = startingPosition;
        //gameObject.transform.rotation = startingRotation;
    }

    public IngredientItemData GetIngredientItemData()
    {
        return data;
    }
}
