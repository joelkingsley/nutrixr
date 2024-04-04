using System.Linq;
using TMPro;
using UnityEngine;

public class FoodItemCanvas : MonoBehaviour
{
    private TMP_Text _nameComponent;
    private TMP_Text _proteinTextComponent;
    private TMP_Text _carbohydratesTextComponent;
    private TMP_Text _fatsTextComponent;
    private TMP_Text _sugarTextComponent;
    private TMP_Text _caloriesTextComponent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeCanvas()
    {
        // Setup text fields
        var textFields = gameObject.GetComponentsInChildren<TMP_Text>();
        _nameComponent = textFields.First(x => x.gameObject.name == "Name");
        _proteinTextComponent = textFields.First(x => x.gameObject.name == "ProteinValue");
        _carbohydratesTextComponent = textFields.First(x => x.gameObject.name == "CarbohydratesValue");
        _fatsTextComponent = textFields.First(x => x.gameObject.name == "FatsValue");
        _sugarTextComponent = textFields.First(x => x.gameObject.name == "SugarValue");
        _caloriesTextComponent = textFields.First(x => x.gameObject.name == "CaloriesValue");

        // Populate text fields
        var ingredientItem = gameObject.transform.parent.GetComponent<IngredientItem>();
        var ingredientItemData = ingredientItem.GetIngredientItemData();
        _nameComponent.text = ingredientItemData.name;
        _proteinTextComponent.text = $"{ingredientItemData.protein} g";
        _carbohydratesTextComponent.text = $"{ingredientItemData.carbohydrates} g";
        _fatsTextComponent.text = $"{ingredientItemData.fat} g";
        _sugarTextComponent.text = $"{ingredientItemData.sugar} g";
        _caloriesTextComponent.text = $"{ingredientItemData.caloriesInKcal} kcal";
    }
}
