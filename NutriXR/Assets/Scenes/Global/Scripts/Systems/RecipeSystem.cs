
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeSystem : MonoBehaviour
{
    [SerializeField]  public GameObject recipeUIScrollViewContent;

    private Ingredient[] selectedIngredients;     //Array of all selected Ingredients
    private Recipe[] allRecipes;            //Array of all recipes
    private List<Recipe> possibleRecipes;   //List of all recipes that are possible

    [SerializeField] private BasketSystem basketSystem;
    [SerializeField] private GameObject recipeEntryPrefab;
    [SerializeField] private GameObject categoryPrefab;

    [SerializeField] private TableItemSpawner recipeSpawner;
    private List<GameObject> cookedRecipes = new();

    void Start()
    {
        allRecipes = Resources.LoadAll<Recipe>("Ingredients/ScriptableObjects");
    }

    /// <summary>
    /// This method checks which Recipes can be made from the Ingredients bought by the user
    /// </summary>
    private void CheckRecipes()
    {
        selectedIngredients = basketSystem.selectedItems.Select(x => x.ingredient).ToArray();
        possibleRecipes = new List<Recipe>();

        foreach (Recipe recipe in allRecipes)
        {
            //Check if Recipe is possible
            bool possible = true;
            foreach (Ingredient ingredient in recipe.ingredients)
            {
                //Check if this ingredient was selected by user
                bool contained = false;
                foreach (Ingredient selectedIngredient in selectedIngredients)
                {
                    if (ingredient.name.Equals(selectedIngredient.name))
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
        //possibleRecipes now holds a List of all Recipes that can be made from the bough Ingredients
    }

    public void RedrawRecipeUI()
    {
        /* DEACTIVATED FOR DEBUGGING PURPOSES
        //clear the UI
        foreach(Transform child in recipeUIScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        CheckRecipes();

        //Redraw GUI
        int numberOfAddedCategories = 0;//this is used to adjust the offset of the elements
        for (int index = 0; index < possibleRecipes.Count; index++)
        {
            bool recipeFinished = true;
            Recipe item = possibleRecipes[index];
            GameObject recipeEntry = Instantiate(recipeEntryPrefab, recipeUIScrollViewContent.transform);
            recipeEntry.transform.GetChild(1).GetComponent<Cooking>().recipeSystem = this;
            recipeEntry.transform.GetChild(1).GetComponent<Cooking>().recipeData = item;
            recipeEntry.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            RectTransform mAnchoredPosition = recipeEntry.GetComponent<RectTransform>();
            float x = mAnchoredPosition.anchoredPosition.x;
            float y = mAnchoredPosition.anchoredPosition.y;
            mAnchoredPosition.anchoredPosition = new Vector2(x+52, y - (30 * index + 10*numberOfAddedCategories)-15);
            for (int j = 0; j < categoriesInPossibleRecipes[index].Count; j++)
            {
                int categoryId = categoriesInPossibleRecipes[index][j];
                GameObject newCategoryTextGameObject = Instantiate(categoryPrefab, recipeEntry.transform);
                newCategoryTextGameObject.AddComponent<TextMeshProUGUI>();

                RectTransform categoryPos = newCategoryTextGameObject.GetComponent<RectTransform>();
                float categoryX = categoryPos.anchoredPosition.x;
                float categoryY = categoryPos.anchoredPosition.y;
                categoryPos.anchoredPosition = new Vector2(categoryX+10, categoryY - (10 * j));
                TextMeshProUGUI textComponent = newCategoryTextGameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = dataStorage.GetCategoryName(categoryId);
                bool foundIngrediendCategory = false;
                foreach (IngredientItem cSelectedItem in basketSystem.selectedItems)
                {
                    //if (cSelectedItem.GetIngredientItemData().categoryIds.Contains(categoryId))
                    //{
                    //    textComponent.color = Color.green;
                    //    foundIngrediendCategory = true;
                    //}
                }

                if (!foundIngrediendCategory) recipeFinished = false;
                numberOfAddedCategories += 1; //this is used to adjust the offset of the elements
            }

            if (recipeFinished)
            {
                recipeEntry.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                recipeEntry.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        */
    }

    public void startCooking(Recipe recipe)
    {
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
