using System;
using System.Collections.Generic;



[Serializable]
public class CategoryDataObject
{
    public int categoryId;
    public string name;
    public List<string> fdcNamesOfIngredientChoices;
}
