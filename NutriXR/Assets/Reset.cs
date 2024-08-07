using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] private BasketRecipeSystem pot;
    [SerializeField] private BasketRecipeSystem oven;
    [SerializeField] private BasketRecipeSystem bowl;
    [SerializeField] private TableItemSpawner tableItemSpawner;
    [SerializeField] private TableItemSpawner fridgeItemSpawner;

    public void reset()
    {
        DataLogger.Log("Reset", "Reset ingredients");
        tableItemSpawner.ResetItems();
        fridgeItemSpawner.ResetItems();
        pot.resetBasket();
        oven.resetBasket();
        bowl.resetBasket();
    }
}
