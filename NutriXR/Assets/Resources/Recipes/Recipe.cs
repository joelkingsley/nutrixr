using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "NutriXR/Recipe")]
public class Recipe : ScriptableObject
{
    public Ingredient[] ingredients;
}
