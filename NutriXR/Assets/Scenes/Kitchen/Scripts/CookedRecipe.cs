using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedRecipe : MonoBehaviour
{
    private Transform userHead;
    private KitchenFeedbackSystem feedbackSystem;
    private Recipe recipe;

    private void Start()
    {
        userHead = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        feedbackSystem = GameObject.FindGameObjectWithTag("FeedbackSystem").GetComponent<KitchenFeedbackSystem>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, userHead.position - new Vector3(0,0.1f,0)) < 0.1)
        {
            feedbackSystem.TrackConsumedRecipe(recipe);
            Destroy(gameObject);
        }
    }

    public void setRecipe(Recipe pRecipe)
    {
        recipe = pRecipe;
    }
}
