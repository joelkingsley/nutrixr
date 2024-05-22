using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngredientItem : MonoBehaviour
{
    private IngredientItemData data;
    private BasketSystem basketSystem;
    private DataStorage dataStorage;
    private GameObject shoppingCart;
    private NutriScoreUI nutriScoreUI;
    private EnvScoreUI envScoreUI;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;

    private Collider[] allColliders;
    private float inPotScaleModifier = 0.5f;
    private bool isInPot = false;

    [SerializeField]
    public string fdcName;

    [SerializeField]
    public bool spawnInFridge;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Supermarket"))
        {
            basketSystem = GameObject.FindGameObjectWithTag("BasketSystem").GetComponent<BasketSystem>();
            nutriScoreUI = GameObject.FindGameObjectWithTag("NutriScoreUI").GetComponent<NutriScoreUI>();
            envScoreUI = GameObject.FindGameObjectWithTag("EnvScoreUI").GetComponent<EnvScoreUI>();
        }
        dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        data = dataStorage.ReadIngredientData(fdcName);

        startingPosition = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;

        allColliders = GetComponentsInChildren<Collider>(false);
        var foodItemCanvas = gameObject.GetComponentInChildren<FoodItemCanvas>();
        foodItemCanvas.InitializeCanvas();
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

        nutriScoreUI.setNutriScore(data.Nutriscorevalue);
        envScoreUI.ShowEnvScore(data.environmentScore);
        //transform.parent = null;
        //shoppingCart.GetComponentInParent<CartSync>().RemoveItemFromCart(this);
        //basketSystem.RemoveFromCart(this);
    }

    public void OnUnselect()
    {
        ChangeAllLayers("PendingIngredientItem");

        nutriScoreUI.setNutriScore('Z');
        envScoreUI.HideScore();
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

        if (other.gameObject.CompareTag("PotEntry") && !isInPot)
        {
            other.transform.GetComponentInParent<BasketSystem>().AddToCart(this);
            transform.localScale *= inPotScaleModifier;
            transform.SetParent(other.GetComponentInParent<Transform>(), true);
            isInPot = true;
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

        if (other.gameObject.CompareTag("PotExit") && isInPot)
        {
            transform.parent = null;
            transform.localScale = startingScale;
            other.transform.GetComponentInParent<BasketSystem>().RemoveFromCart(this);
            isInPot = false;
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
