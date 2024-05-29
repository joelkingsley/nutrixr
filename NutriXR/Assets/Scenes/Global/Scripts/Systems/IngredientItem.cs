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

        scoreUI.ShowNutriScore(ingredient.nutriScore);
        //scoreUI.ShowEnvScore(ingredient.environmentScore);

        //transform.parent = null;
        //shoppingCart.GetComponentInParent<CartSync>().RemoveItemFromCart(this);
        //basketRecipeSystem.RemoveFromCart(this);
    }

    public void OnUnselect()
    {
        ChangeAllLayers("PendingIngredientItem");
        scoreUI.Hide();
    }

    private void OnTriggerEnter(Collider other)
    {
        //UnselectedIngredientItem: No behaviour
        //SelectedIngredientItem: No behaviour
        //ShoppingCart: No behaviour

        if (shoppingCart == null)
        {
            shoppingCart = GameObject.FindGameObjectWithTag("ShoppingCartItemHook");
            basketRecipeSystem = shoppingCart.GetComponentInParent<CartSync>().GetComponentInChildren<BasketRecipeSystem>();
        }

        if (gameObject.layer == LayerMask.NameToLayer("PendingIngredientItem")
            && other.gameObject.layer == LayerMask.NameToLayer("ShoppingCart"))
        {
            //PendingIngredientItem <-> ShoppingCart: The pending item becomes part of shopping cart
            transform.SetParent(shoppingCart.transform, true);
            shoppingCart.GetComponentInParent<CartSync>().AddItemToCart(this);
            basketRecipeSystem.AddToBasket(this);
            ChangeAllLayers("ShoppingCart");
        }

        if (other.gameObject.CompareTag("PotEntry") && !isInPot)
        {
            other.transform.GetComponentInParent<BasketRecipeSystem>().AddToBasket(this);
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
            basketRecipeSystem.RemoveFromBasket(this);
            if (gameObject.layer != LayerMask.NameToLayer("SelectedIngredientItem"))
            {
                ChangeAllLayers("PendingIngredientItem");
            }
        }

        if (other.gameObject.CompareTag("PotExit") && isInPot)
        {
            transform.parent = null;
            transform.localScale = startingScale;
            other.transform.GetComponentInParent<BasketRecipeSystem>().RemoveFromBasket(this);
            isInPot = false;
        }

    }
}
