using System;
using System.Collections.Generic;

[Serializable]
public class EnvFeedbackDataObject
{
    public FoodData food;
    public Co2Data co2;
    public LandData land;
    public WaterData water;

    [Serializable]
    public class FoodData
    {
        public string name;
        public int weight;
        public string unit;
    }

    [Serializable]
    public class Co2Data
    {
        public  string value;
        public int weight;
        public string unit;
    }

    [Serializable]
    public class LandData
    {
        public string name;
        public int weight;
        public string unit;
    }

    [Serializable]
    public class WaterData
    {
        public string name;
        public int weight;
        public string unit;
    }
}
