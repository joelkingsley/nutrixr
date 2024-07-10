using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class IngredientItem : MonoBehaviour
{
    public Ingredient ingredient;

    private BasketRecipeSystem basketRecipeSystem;
    private GameObject shoppingCart;
    private ScoreUI scoreUI;

    private Vector3 startingScale;
    private Transform startingParent;

    private Collider[] allColliders;
    private float inPotScaleModifier = 0.5f;
    private bool isInPot = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Supermarket"))
        {
            scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUI>();
        }

        startingScale = transform.localScale;
        startingParent = transform.parent;

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
        if (!SceneManager.GetActiveScene().name.Equals("Supermarket")) return;
        ChangeAllLayers("SelectedIngredientItem");

        DataLogger.Log("IngredientItem", "Grabbed " + name + ".");

        scoreUI.Show(ingredient);
    }

    public void OnUnselect()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Supermarket")) return;
        ChangeAllLayers("PendingIngredientItem");

        DataLogger.Log("IngredientItem", "Dropped " + name + ".");

        scoreUI.Hide();
    }

    private void OnTriggerEnter(Collider other)
    {
        //UnselectedIngredientItem: No behaviour
        //SelectedIngredientItem: No behaviour
        //ShoppingCart: No behaviour

        if (SceneManager.GetActiveScene().name.Equals("Supermarket"))
        {
            if (shoppingCart == null)
            {
                shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartItemHook");
                Debug.Log(other.gameObject.name);
                basketRecipeSystem = shoppingCart.GetComponentInParent<CartSync>().GetComponentInChildren<BasketRecipeSystem>();
            }

            if (gameObject.layer == LayerMask.NameToLayer("PendingIngredientItem")
                && other.gameObject.layer == LayerMask.NameToLayer("ShoppingCart"))
            {
                //PendingIngredientItem <-> ShoppingCart: The pending item becomes part of shopping cart
                transform.SetParent(shoppingCart.transform, true);
                shoppingCart.GetComponentInParent<CartSync>().AddItemToCart(this);
                basketRecipeSystem.AddToBasket(this);
                DataLogger.Log("IngredientItem", name + " collided with shopping cart volume in supermarket.");
                ChangeAllLayers("ShoppingCart");
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("Kitchen"))
        {
            if (other.gameObject.CompareTag("PotEntry") && !isInPot)
            {
                other.transform.parent.parent.GetComponentInChildren<BasketRecipeSystem>().AddToBasket(this);
                transform.localScale *= inPotScaleModifier;
                transform.SetParent(other.GetComponentInParent<Transform>(), true);
                isInPot = true;
                DataLogger.Log("IngredientItem", name + " collided with pot volume in kitchen.");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("PendingIngredientItem")
            && other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            //PendingIngredientItem <-> Default: The pending item is dropped and thus unselected
            ChangeAllLayers("UnselectedIngredientItem");
            DataLogger.Log("IngredientItem", name + " dropped on floor.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneManager.GetActiveScene().name.Equals("Supermarket"))
        {
            if (shoppingCart == null)
            {
                shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartItemHook");
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("ShoppingCart"))
            {
                transform.SetParent(startingParent, true);
                shoppingCart.GetComponentInParent<CartSync>().RemoveItemFromCart(this);
                basketRecipeSystem.RemoveFromBasket(this);
                DataLogger.Log("IngredientItem", name + " exited shopping cart volume in supermarket.");
                if (gameObject.layer != LayerMask.NameToLayer("SelectedIngredientItem"))
                {
                    ChangeAllLayers("PendingIngredientItem");
                }
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("Kitchen"))
        {
            if (other.gameObject.CompareTag("PotExit") && isInPot)
            {
                transform.parent = null;
                transform.localScale = startingScale;
                other.transform.parent.parent.GetComponentInChildren<BasketRecipeSystem>().RemoveFromBasket(this);
                isInPot = false;
            }
        }
    }
}
