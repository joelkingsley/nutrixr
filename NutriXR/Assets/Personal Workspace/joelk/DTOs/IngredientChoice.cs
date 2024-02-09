using System;
using System.Collections.Generic;

namespace Personal_Workspace.joelk.DTOs
{
    [Serializable]
    public class IngredientChoice
    {
        public string fdcName;

        [Serializable]
        public class FdcData
        {
            public string fdcId;
            public List<FdcFoodNutrient> foodNutrients;

            [Serializable]
            public class FdcFoodNutrient
            {
                public string type = "FoodNutrient";
                public double id;
                public FdcNutrient nutrient;
                public int dataPoints;
                public float max;
                public float min;
                public float median;
                public float amount;

                [Serializable]
                public class FdcNutrient
                {
                    public double id;
                    public string number;
                    public string name;
                    public double rank;
                    public string unitName;
                }
            }
        }
    }
}
