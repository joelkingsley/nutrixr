namespace a;

public class RecipeItemData
{
   public int id;
   public string name;
   public List<(int, float)> categoryIdsWithWeights;
   public float calories;
   public float caloriesInKcal;
   public float carbohydrates;
   public float protein;
   public float fat;
   public float sugar;
   public float recipe_calories;
   public float recipe_caloriesInKcal;
   public float recipe_carbohydrates;
   public float recipe_protein;
   public float recipe_fat;
   public float recipe_sugar;
   public class RecipeNuritionCalculation(RecipeDataObject recipeDataObject, IngredientDataObject ingredientDataObject)
   {
       recipeName = recipeDataObject.name;
       recipeId = recipeDataObject.id;
       categoryIdsWithWeights = recipeDataObject.inputIngredientCategories.Select(x => (x.id, x.ingredientWeight)).ToList();
       foreach (var ingredient in ingredientDataObject){
           ingName = ingredient.name;
           ingfdcName = ingredient.fdcName;
           var fatIndex = ingredientDataObject.data.foodNutrients.FindIndex(
               nutrient => nutrient.nutrient.name == "Total fat (NLEA)" | nutrient.nutrient.name == "Total lipid (fat)");
           if (fatIndex >= 0)
           {
               fat = ingredientDataObject.data.foodNutrients[fatIndex].amount;
           }
           var sugarIndex = ingredientDataObject.data.foodNutrients.FindIndex(
               nutrient => nutrient.nutrient.name == "Sugars, total including NLEA" | nutrient.nutrient.name == "Sugars, Total");
           if (sugarIndex >= 0)
           {
               sugar = ingredientDataObject.data.foodNutrients[sugarIndex].amount;
           }
           var carbohydratesIndex = ingredientDataObject.data.foodNutrients.FindIndex(
               nutrient => nutrient.nutrient.name == "Carbohydrate, by difference");
           if (carbohydratesIndex >= 0)
           {
               carbohydrates = ingredientDataObject.data.foodNutrients[carbohydratesIndex].amount;
           }
           var proteinIndex = ingredientDataObject.data.foodNutrients.FindIndex(
               nutrient => nutrient.nutrient.name == "Protein");
           if (proteinIndex >= 0)
           {
               protein = ingredientDataObject.data.foodNutrients[proteinIndex].amount;
           }
           var caloriesIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
               nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kJ");
           if (caloriesIndex >= 0)
           {
               calories = ingredientDataObject.data.foodNutrients[caloriesIndex].amount;
           }
           var caloriesInKcalIndex = ingredientDataObject.data.foodNutrients.FindIndex(nutrient =>
               nutrient.nutrient.name == "Energy" & nutrient.nutrient.unitName == "kcal");
           if (caloriesInKcalIndex >= 0)
           {
               caloriesInKcal = ingredientDataObject.data.foodNutrients[caloriesInKcalIndex].amount;
           }

           recipe_calories += calories;
           recipe_caloriesInKcal += caloriesInKcal;
           recipe_carbohydrates += carbohydrates;
           recipe_protein += protein;
           recipe_fat += fat;
           recipe_sugar += sugar;
       }
   }
}

