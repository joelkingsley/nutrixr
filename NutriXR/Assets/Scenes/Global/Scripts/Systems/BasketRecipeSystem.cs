
using System;
using System.Collections.Generic;
using System.Linq;
using Realms;
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

    private List<IngredientItem> ingredientItemsInBasket = new List<IngredientItem>(); //Array of all stored ingredients

    private Dictionary<string, Ingredient> allIngredients = new Dictionary<string, Ingredient>();
    private Dictionary<string, Recipe> allRecipes = new Dictionary<string, Recipe>();

    [SerializeField] private TableItemSpawner recipeSpawner = null;
    private bool recipesCanBePrepared = false;
    private List<GameObject> preparedRecipes = new();

    public struct RecipeIngredientRenderData
    {
        public Recipe recipeData;
        public List<Tuple<Ingredient, bool>> ingredientsAvailableData;
        public bool preparable; // recipe can be cooked

        public int ingredientsMissingCount;

        public RecipeIngredientRenderData(Recipe r, List<Tuple<Ingredient, bool>> ingAvailData, bool p)
        {
            recipeData = r;
            ingredientsAvailableData = ingAvailData;
            preparable = p;

            ingredientsMissingCount = 0;
            foreach (Tuple<Ingredient, bool> pair in ingAvailData)
            {
                if (pair.Item2 == false)
                {
                    ingredientsMissingCount += 1;
                }
            }
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        List<Ingredient> loadedIngredientList = new List<Ingredient>(Resources.LoadAll<Ingredient>("Ingredients/ScriptableObjects"));
        List<Recipe> loadedRecipeList = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes/ScriptableObjects"));

        foreach (Ingredient ingredient in loadedIngredientList)
        {
            allIngredients.Add(ingredient.name, ingredient);
        }

        foreach (Recipe recipe in loadedRecipeList)
        {
            allRecipes.Add(recipe.name, recipe);
        }

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

    private Dictionary<string, int> GetBasketRenderData()
    {
        Dictionary<string, int> ingredientCountDict = new Dictionary<string, int>();

        foreach (IngredientItem item in ingredientItemsInBasket)
        {
            Ingredient ingredient = item.ingredient;
            if (!ingredientCountDict.ContainsKey(ingredient.name))
            {
                ingredientCountDict.Add(ingredient.name, 0);
            }

            ingredientCountDict[ingredient.name] += 1;
        }

        return ingredientCountDict;
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

        Dictionary<string, int> basketEntryCountDict = GetBasketRenderData();
        List<Tuple<Ingredient, int>> basketRenderData = new List<Tuple<Ingredient, int>>();

        foreach(KeyValuePair<string, int> kv in basketEntryCountDict)
        {
            basketRenderData.Add(new Tuple<Ingredient, int>(allIngredients[kv.Key], kv.Value)); // load ingredient from ingredients dict
        }
        basketRenderData.Sort((Tuple<Ingredient, int> x, Tuple<Ingredient, int> y) =>
        {
            return x.Item1.name.CompareTo(y.Item1.name);
        });

        foreach (Tuple<Ingredient, int> ingredientCountPair in basketRenderData)
        {
            spawnBasketUIEntry(ingredientCountPair.Item1, ingredientCountPair.Item2);
        }
    }

    private void spawnBasketUIEntry(Ingredient itemData, int ingredientCount)
    {
        GameObject newIngredientElement = GameObject.Instantiate(basketUIIngredientElementTemplate, basketUIMenuBody.transform);
        newIngredientElement.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = itemData.name + " x" + ingredientCount;
        newIngredientElement.SetActive(true);
    }

    private List<RecipeIngredientRenderData> GetRecipesToShow()
    {
        List<RecipeIngredientRenderData> recipesToShow = new List<RecipeIngredientRenderData>();

        foreach (KeyValuePair<string, Recipe> kv in allRecipes)
        {
            Recipe recipe = kv.Value;

            List<Tuple<Ingredient, bool>> ingredientsAvailableData = new List<Tuple<Ingredient, bool>>();
            int ingredientsAvailable = 0;

            foreach (Ingredient recipeIngredient in recipe.ingredients)
            {
                bool contained = false;
                foreach (IngredientItem selectedItem in ingredientItemsInBasket)
                {
                    if (recipeIngredient.name.Equals(selectedItem.ingredient.name))
                    {
                        contained = true;
                        break;
                    }
                }

                Tuple<Ingredient, bool> available = new Tuple<Ingredient, bool>(recipeIngredient, contained);
                ingredientsAvailableData.Add(available);
                if (contained)
                {
                    ingredientsAvailable += 1;
                }
            }

            if (ingredientsAvailable > 0)
            {
                recipesToShow.Add(new RecipeIngredientRenderData(recipe, ingredientsAvailableData, recipe.ingredients.Length == ingredientsAvailable));
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

        List<RecipeIngredientRenderData> recipesToShow = GetRecipesToShow();
        recipesToShow.Sort((RecipeIngredientRenderData x, RecipeIngredientRenderData y) =>
        {
            if (x.ingredientsMissingCount == y.ingredientsMissingCount)
            {
                return x.recipeData.name.CompareTo(y.recipeData.name);
            }
            else
            {
                return x.ingredientsMissingCount - y.ingredientsMissingCount;
            }
        });

        //Redraw GUI
        foreach (RecipeIngredientRenderData recipeData in recipesToShow)
        {
            spawnRecipeUIEntry(recipeData);
        }
    }

    private void cookBtnClicked(Recipe recipe)
    {
        Debug.Log("Start cooking " + recipe.name);
        startCooking(recipe);
    }

    private void spawnRecipeUIEntry(RecipeIngredientRenderData renderData)
    {
        Recipe recipeData = renderData.recipeData;
        List<Tuple<Ingredient, bool>> ingredientsAvailableData = renderData.ingredientsAvailableData;
        bool recipePossible = renderData.preparable;

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

        foreach (Tuple<Ingredient, bool> ingredientAvailablePair in ingredientsAvailableData)
        {
            Ingredient ingredient = ingredientAvailablePair.Item1;
            bool ingredientAvailable = ingredientAvailablePair.Item2;

            GameObject newIngredientElement =
                GameObject.Instantiate(recipeUIIngredientElementTemplate, recipeUIMenuBody.transform);
            newIngredientElement.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ingredient.name;
            newIngredientElement.SetActive(true);

            if (ingredientAvailable)
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
