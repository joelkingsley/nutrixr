using System;
using System.Collections.Generic;


    [Serializable]
    public class IngredientDataObject
    {
        public string fdcName;
        public string name;
        public string foodGroup;
        public FdcData data;
        public int[] categoryIds;

        [Serializable]
        public class FdcData
        {
            public string fdcId;
            public List<FdcFoodNutrient> foodNutrients;

            [Serializable]
            public class FdcFoodNutrient
            {
                public string type;
                public double id;
                public FdcNutrient nutrient;
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

