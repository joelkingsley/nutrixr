
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasketRecipeSystem : MonoBehaviour
{
    [SerializeField] private GameObject basketUIMenuBody;
    [SerializeField] private GameObject recipeUIMenuBody;
    // template UI elements
    [SerializeField] private GameObject basketUIIngredientElementTemplate;
    [SerializeField] private GameObject recipeUIRecipeEntryTemplate;
    [SerializeField] private GameObject recipeUIIngredientElementTemplate;

    private List<IngredientItem> ingredientItemsInBasket; //Array of all stored ingredients
    private List<Recipe> allRecipes;  //Array of all recipes

    [SerializeField] private TableItemSpawner recipeSpawner = null;
    private bool recipesCanBePrepared = false;
    private List<GameObject> preparedRecipes = new();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        ingredientItemsInBasket = new List<IngredientItem>();

        allRecipes = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes/ScriptableObjects"));
        if (recipeSpawner != null)
        {
            recipesCanBePrepared = true;
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if (scene.name == "Kitchen")
        {
            //Un-cooled Ingredients
            List<Ingredient> tableItemDatas = new List<Ingredient>();
            foreach (IngredientItem item in ingredientItemsInBasket)
            {
                if (item.ingredient.cooled == false)
                {
                    tableItemDatas.Add(item.ingredient);
                }
            }
            GameObject.FindGameObjectWithTag("KitchenTableIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(tableItemDatas);

            //Cooled Ingredients
            List<Ingredient> fridgeItemDatas = new List<Ingredient>();
            foreach (IngredientItem item in ingredientItemsInBasket)
            {
                if (item.ingredient.cooled)
                {
                    fridgeItemDatas.Add(item.ingredient);
                }
            }
            GameObject.FindGameObjectWithTag("KitchenFridgeIngredientSpawner").GetComponent<TableItemSpawner>().SpawnKitchenSceneItems(fridgeItemDatas);
        }
    }

    public void AddToBasket(IngredientItem ingredientItem)
    {
        if (ingredientItem == null)
        {
            Debug.LogError("Tried to add a null Ingredient to basket");
            return;
        }
        //TODO NullPointer

        if (ingredientItem.ingredient.name.Equals(""))
        {
            Debug.Log("Item without a name, wont be added to list");
            return;
        }
        ingredientItemsInBasket.Add(ingredientItem);

        RedrawRecipeUI();
        RedrawBasketUI();
    }

    public void RemoveFromBasket(IngredientItem ingredientItem)
    {
        if (ingredientItem == null)
        {
            Debug.LogError("Tried to remove a null Ingredient from a basket");
            return;
        }
        ingredientItemsInBasket.Remove(ingredientItem);
        RedrawRecipeUI();
        RedrawBasketUI();
    }

    public void RedrawBasketUI()
    {
        foreach(Transform child in basketUIMenuBody.transform)
        {
            if (child.gameObject.activeSelf) // to not delete the template element
            {
                Destroy(child.gameObject);
            }
        }

        foreach (IngredientItem item in ingredientItemsInBasket)
        {
            spawnBasketUIEntry(item.ingredient);
        }
    }

    private void spawnBasketUIEntry(Ingredient itemData)
    {
        GameObject newIngredientElement = GameObject.Instantiate(basketUIIngredientElementTemplate, basketUIMenuBody.transform);
        newIngredientElement.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = itemData.name;
        newIngredientElement.SetActive(true);
    }

    /// <summary>
    /// This method checks which Recipes can be made from the Ingredients bought by the user
    /// </summary>
    private List<Recipe> GetPossibleRecipes()
    {
        List<Recipe> possibleRecipes = new List<Recipe>();

        foreach (Recipe recipe in allRecipes)
        {
            //Check if Recipe is possible
            bool possible = true;
            foreach (Ingredient ingredient in recipe.ingredients)
            {
                //Check if this ingredient was selected by user
                bool contained = false;
                foreach (IngredientItem selectedItem in ingredientItemsInBasket)
                {
                    if (ingredient.name.Equals(selectedItem.ingredient.name))
                    {
                        contained = true;
                        break;
                    }
                }
                if (!contained)
                {
                    //Ingredient is not contained. The recipe is thus not possible
                    possible = false;
                    break;
                }
            }

            //Recipe is possible to make with the bough Ingredients
            if (possible)
            {
                possibleRecipes.Add(recipe);
            }
        }

        return possibleRecipes;
        //possibleRecipes now holds a List of all Recipes that can be made from the bough Ingredients
    }

    private List<Recipe> GetRecipesToShow()
    {
        List<Recipe> recipesToShow = new List<Recipe>();

        foreach (Recipe recipe in allRecipes)
        {
            bool contained = false;
            foreach (Ingredient recipeIngredient in recipe.ingredients)
            {
                foreach (IngredientItem selectedItem in ingredientItemsInBasket)
                {
                    if (recipeIngredient.name.Equals(selectedItem.ingredient.name))
                    {
                        recipesToShow.Add(recipe);
                        contained = true;
                        break;
                    }
                }

                if (contained) break;
            }
        }

        return recipesToShow;
    }

    public void RedrawRecipeUI()
    {
        foreach(Transform child in recipeUIMenuBody.transform)
        {
            if (child.gameObject.activeSelf) // to not delete the template element
            {
                Destroy(child.gameObject);
            }
        }

        List<Recipe> possibleRecipes = GetPossibleRecipes();
        List<Recipe> recipesToShow = GetRecipesToShow();

        //Redraw GUI
        foreach (Recipe recipeData in recipesToShow)
        {
            bool recipePossible = false;
            foreach (Recipe possibleRecipe in possibleRecipes)
            {
                if (possibleRecipe.name.Equals(recipeData.name))
                {
                    recipePossible = true;
                    break;
                }
            }

            spawnRecipeUIEntry(recipeData, recipePossible);
        }
    }

    private void cookBtnClicked(Recipe recipe)
    {
        Debug.Log("Start cooking " + recipe.name);
        startCooking(recipe);
    }

    private void spawnRecipeUIEntry(Recipe recipeData, bool recipePossible)
    {
        GameObject newRecipeElement = GameObject.Instantiate(recipeUIRecipeEntryTemplate, recipeUIMenuBody.transform);
        newRecipeElement.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = recipeData.name;
        newRecipeElement.SetActive(true);

        if (recipePossible)
        {
            newRecipeElement.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
        }

        if (recipesCanBePrepared)
        {
            newRecipeElement.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                cookBtnClicked(recipeData);
            });
        }
        else
        {
            newRecipeElement.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }

        foreach (Ingredient ingredient in recipeData.ingredients)
        {
            GameObject newIngredientElement =
                GameObject.Instantiate(recipeUIIngredientElementTemplate, recipeUIMenuBody.transform);
            newIngredientElement.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ingredient.name;
            newIngredientElement.SetActive(true);

            bool contained = false;
            foreach (IngredientItem item in ingredientItemsInBasket)
            {
                if (ingredient.name.Equals(item.ingredient.name))
                {
                    contained = true;
                    break;
                }
            }

            if (contained)
            {
                newIngredientElement.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.green;
            }
        }
    }

    public void startCooking(Recipe recipe)
    {
        if (!recipesCanBePrepared) return;
        /* DEACTIVATED FOR DEBUGGING PURPOSES
        //Debug.Log("Cooking: " + recipeData.name);
        GameObject prefab = (GameObject) Resources.Load("RecipePrefabs/" + recipeData.name, typeof(GameObject));
        GameObject recipeObject = Instantiate(prefab);
        cookedRecipes.Add(recipeObject);
        recipeSpawner.addItem(recipeObject);

        foreach (IngredientItem selectedItem in basketSystem.selectedItems)
        {
            List<int> categoryIDs = recipeData.categoryIdsWithWeights.Select(x => x.Item1).ToList();
            foreach (int categoryId in categoryIDs)
            {
                if (selectedItem.GetIngredientItemData().categoryIds.Contains(categoryId))
                {
                    recipeObject.GetComponent<RecipeIngredients>().ingredients.Add(selectedItem.fdcName);
                    basketSystem.RemoveFromCart(selectedItem);
                    Destroy(selectedItem.gameObject);
                    return;
                }
            }
        }
        */
    }
}
