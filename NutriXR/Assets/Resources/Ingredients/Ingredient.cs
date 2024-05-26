using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "NutriXR/Ingredient")]
public class Ingredient : ScriptableObject
{

    public enum NutriScore { A,B,C,D,E, None }
    public enum EnvScore { Green,Yellow,Red, None }
    public enum FoodGroup { FruitVegetable, Juice, Legume, NutSeed, Potato, GrainBreadNoodle, PlantOil, Butter, Milk, Fish, Meat, Sausage, Egg, None }

    //###   NutriScore  ###
    [Header("Nutritional Health")]

    [Tooltip("NutriScore as introduced by EU")]
    public NutriScore nutriScore;
    [Tooltip("Food group as introduced in DGE Nutrition Circle")]
    public FoodGroup foodGroup;

    [Space(10)]

    //###   Environment ###
    [Header("Environmental Impact (Based on 100g)")]

    [Tooltip("Traffic Light system for evaluating environmental impact")]
    public EnvScore environmentScore;
    [Tooltip("Kilograms")]
    public float CO2Emission;   //In Kg
    [Tooltip("Meter squared")]
    public float LandUse;       //In m^2
    [Tooltip("Liter")]
    public float WaterUse;      //In L

    [Space(10)]

    //###   Extras    ###
    [Header("Extras")]
    [Tooltip("Does this ingredient need to be cooled?")]
    public bool cooled;
}
