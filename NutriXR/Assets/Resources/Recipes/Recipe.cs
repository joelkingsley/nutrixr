using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "NutriXR/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("List of Ingredients contained. Order matters")]
    public Ingredient[] ingredients;

    [Header("Weight in (g/ml) of ingredient. Order matters")]
    public float[] weights;
}
