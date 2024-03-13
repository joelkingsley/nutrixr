using System.Collections.Generic;
using System.Linq;

namespace Personal_Workspace.joelk.Entities
{
    /*public class RecipeItemData:IngredientItemData
    {
        public int Id;
        private List<(IngredientItemData, float)> _ingredientItemsWithWeight;

        public RecipeItemData(int id, string name, List<(IngredientItemData, float)> ingredientItemsWithWeight)
        {
            Id = id;
            this.name = name;
            _ingredientItemsWithWeight = ingredientItemsWithWeight;
            isIngredient = false;

            var totalWeight = _ingredientItemsWithWeight.Sum(x => x.Item2);
            var proteinSum = 0.0f;
            var carbohydratesSum = 0.0f;
            var fatsSum = 0.0f;
            var sugarSum = 0.0f;
            var caloriesSum = 0.0f;
            var caloriesInKcalSum = 0.0f;
            foreach (var tuple in _ingredientItemsWithWeight)
            {
                var itemProportion = (tuple.Item2 / totalWeight);
                proteinSum += itemProportion * tuple.Item1.protein;
                carbohydratesSum += itemProportion * tuple.Item1.carbohydrates;
                fatsSum += itemProportion * tuple.Item1.fat;
                sugarSum += itemProportion * tuple.Item1.sugar;
                caloriesSum += itemProportion * tuple.Item1.calories;
                caloriesInKcalSum += itemProportion * tuple.Item1.caloriesInKcal;
            }

            protein = proteinSum;
            carbohydrates = carbohydratesSum;
            fat = fatsSum;
            sugar = sugarSum;
            calories = caloriesSum;
            caloriesInKcal = caloriesInKcalSum;
        }
    }*/
}
