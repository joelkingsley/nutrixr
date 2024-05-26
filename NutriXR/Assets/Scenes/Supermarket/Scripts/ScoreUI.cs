using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Org.BouncyCastle.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private GameObject circles;
    [SerializeField] private GameObject text;
    [SerializeField] private List<GameObject> backgrounds;
    private Dictionary<Ingredient.NutriScore, GameObject> colorDict;

    private void Update()
    {
        transform.position = hand.position + new Vector3(0.07f, 0.07f, 0);
        Vector3 eyeDirection = eyeCenter.position - transform.position;
        transform.localRotation = Quaternion.LookRotation(-eyeDirection.normalized, transform.up);
        transform.localRotation.Set(transform.rotation.x+90, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void Start()
    {
        colorDict = new Dictionary<Ingredient.NutriScore, GameObject>(); //0:A, 1:B, 2:C, 3:D, 4:E
        colorDict.Add(Ingredient.NutriScore.A, backgrounds[0]);
        colorDict.Add(Ingredient.NutriScore.B, backgrounds[1]);
        colorDict.Add(Ingredient.NutriScore.C, backgrounds[2]);
        colorDict.Add(Ingredient.NutriScore.D, backgrounds[3]);
        colorDict.Add(Ingredient.NutriScore.E, backgrounds[4]);

        Hide();
    }

    public void ShowNutriScore(Ingredient.NutriScore score)
    {
        if (score == Ingredient.NutriScore.None)
        {
            circles.SetActive(false);
            text.SetActive(false);
            return;
        }

        circles.SetActive(true);
        text.SetActive(true);
        disableAll();
        colorDict[score].SetActive(true);
        text.GetComponent<TextMeshProUGUI>().text = Enum.GetName(typeof(Ingredient.NutriScore), score);
    }

    public void ShowEnvScore(Ingredient.EnvScore score)
    {
        text.SetActive(false);
        if (score == Ingredient.EnvScore.None)
        {
            circles.SetActive(false);
            return;
        }

        circles.SetActive(true);
        disableAll();
        //We misuse the NutriScore to show the EnvScore
        switch (score)
        {
            case Ingredient.EnvScore.Green:
                colorDict[Ingredient.NutriScore.A].SetActive(true);
                break;
            case Ingredient.EnvScore.Yellow:
                colorDict[Ingredient.NutriScore.C].SetActive(true);
                break;
            case Ingredient.EnvScore.Red:
                colorDict[Ingredient.NutriScore.E].SetActive(true);
                break;
            default:
                circles.SetActive(false);
                text.SetActive(false);
                break;
        }
    }

    public void Hide()
    {
        circles.SetActive(false);
        text.SetActive(false);
    }

    private void disableAll()
    {
        foreach (GameObject inner in backgrounds)
        {
            inner.SetActive(false);
        }
    }
}
