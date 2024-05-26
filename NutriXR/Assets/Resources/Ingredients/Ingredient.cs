using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "NutriXR/Ingredient")]
public class Ingredient : ScriptableObject
{
    //public string ingredientName;
    public enum NutriScore { A,B,C,D,E, None }
    public NutriScore nutriScore;

    public enum EnvScore { Green,Yellow,Red, None }
    public EnvScore environmentScore;

    public bool cooled;

}
