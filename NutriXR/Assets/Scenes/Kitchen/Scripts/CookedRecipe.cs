using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedRecipe : MonoBehaviour
{
    private Transform userHead;
    private KitchenFeedbackSystem feedbackSystem;
    private AudioSource audioSource;
    private Rigidbody rb;
    private Recipe recipe;
    public int destroyTimer ;

    private void Start()
    {
        userHead = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        feedbackSystem = GameObject.FindGameObjectWithTag("FeedbackSystem").GetComponent<KitchenFeedbackSystem>();
    }

    private void FixedUpdate()
    {
        if (destroyTimer > 0)
        {
            destroyTimer--;
            if (destroyTimer == 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        else if (Vector3.Distance(transform.position, userHead.position - new Vector3(0,0.1f,0)) < 0.15)
        {
            feedbackSystem.TrackConsumedRecipe(recipe);
            audioSource.Play();
            rb.isKinematic = true;
            destroyTimer = 69;
        }
    }

    public void setRecipe(Recipe pRecipe)
    {
        recipe = pRecipe;
    }
}
