using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private GameObject rightCanvas;
    [SerializeField] private GameObject rightCircles;
    [SerializeField] private GameObject leftCanvas;
    [SerializeField] private GameObject leftCircles;
    [SerializeField] private GameObject rightText;
    [SerializeField] private GameObject leftText;
    [SerializeField] private List<GameObject> rightBackgrounds;
    [SerializeField] private List<GameObject> leftBackgrounds;
    private Dictionary<Ingredient.NutriScore, GameObject> rightColorDict;
    private Dictionary<Ingredient.NutriScore, GameObject> leftColorDict;

    private void Start()
    {
        rightCanvas.transform.SetParent(rightHand);
        rightCanvas.transform.localPosition = new Vector3(0.07f, 0.07f, 0);
        leftCanvas.transform.SetParent(leftHand);
        leftCanvas.transform.localPosition = new Vector3(0, 0.07f, 0.07f);

        rightColorDict = new Dictionary<Ingredient.NutriScore, GameObject>();
        rightColorDict.Add(Ingredient.NutriScore.A, rightBackgrounds[0]);
        rightColorDict.Add(Ingredient.NutriScore.B, rightBackgrounds[1]);
        rightColorDict.Add(Ingredient.NutriScore.C, rightBackgrounds[2]);
        rightColorDict.Add(Ingredient.NutriScore.D, rightBackgrounds[3]);
        rightColorDict.Add(Ingredient.NutriScore.E, rightBackgrounds[4]);

        leftColorDict = new Dictionary<Ingredient.NutriScore, GameObject>();
        leftColorDict.Add(Ingredient.NutriScore.A, leftBackgrounds[0]);
        leftColorDict.Add(Ingredient.NutriScore.B, leftBackgrounds[1]);
        leftColorDict.Add(Ingredient.NutriScore.C, leftBackgrounds[2]);
        leftColorDict.Add(Ingredient.NutriScore.D, leftBackgrounds[3]);
        leftColorDict.Add(Ingredient.NutriScore.E, leftBackgrounds[4]);

        Hide(false);
        Hide(true);
    }

    private void Update()
    {
        if (eyeCenter.position != Vector3.zero)   //Prevent Server flooding
        {
            //Adjust right score
            Transform rightTrans = rightCanvas.transform;
            rightTrans.rotation = Quaternion.LookRotation(-(eyeCenter.position - rightTrans.position).normalized, Vector3.up);

            //Adjust left score
            Transform leftTrans = leftCanvas.transform;
            leftTrans.rotation = Quaternion.LookRotation(-(eyeCenter.position - leftTrans.position).normalized, Vector3.up);
        }
    }

    public bool isLeftHandGrab(Vector3 itemPos)
    {
        if (Vector3.Distance(itemPos, leftHand.position) <= Vector3.Distance(itemPos, rightHand.position))
        {
            return true;
        }
        return false;
    }

    private void setNutriscore(Ingredient.NutriScore score, bool isLeftSide)
    {
        if (isLeftSide)
        {
            leftColorDict[score].SetActive(true);
            leftText.GetComponent<TextMeshProUGUI>().text = Enum.GetName(typeof(Ingredient.NutriScore), score);
        } else {
            rightColorDict[score].SetActive(true);
            rightText.GetComponent<TextMeshProUGUI>().text = Enum.GetName(typeof(Ingredient.NutriScore), score);
        }
    }

    private void ShowNutriScore(Ingredient.NutriScore score, bool isLeftGrab)
    {
        bool state = score != Ingredient.NutriScore.None;

        if (isLeftGrab)
        {
            leftCircles.SetActive(state);
        } else {
            rightCircles.SetActive(state);
        }
        //Debug.Log("Set NutriScore: " + state +" with score " + score);

        disableAll(isLeftGrab);
        setNutriscore(score, isLeftGrab);
    }

    private void ShowEnvScore(Ingredient.EnvScore score, bool isLeftGrab)
    {
        Dictionary<Ingredient.NutriScore, GameObject> usedDict = rightColorDict;
        bool state = score != Ingredient.EnvScore.None;
        if (isLeftGrab)
        {
            usedDict = leftColorDict;
            leftCircles.SetActive(state);
            leftText.SetActive(false);
        } else {
            rightCircles.SetActive(state);
            rightText.SetActive(false);
        }

        disableAll(isLeftGrab);
        //We misuse the NutriScore to show the EnvScore
        //Debug.Log("Set EnvScore: " + state +" with score " + score);
        switch (score) {
            case Ingredient.EnvScore.Green:
                usedDict[Ingredient.NutriScore.A].SetActive(true);
                break;
            case Ingredient.EnvScore.Yellow:
                usedDict[Ingredient.NutriScore.C].SetActive(true);
                break;
            case Ingredient.EnvScore.Red:
                usedDict[Ingredient.NutriScore.E].SetActive(true);
                break;
            default:
                rightCircles.SetActive(false);
                rightText.SetActive(false);
                break;
        }
    }

    public void Show(Ingredient ingredient, bool isLeftGrab)
    {
        if (!DataLogger.IsFirstRun)
        {
            if (DataLogger.GOAL == "Nutrition")
            {
                ShowNutriScore(ingredient.nutriScore, isLeftGrab);
                DataLogger.Log("ScoreUI", "Showing NutriScore " + ingredient.nutriScore + " for " + ingredient.name);
            }
            else
            {
                ShowEnvScore(ingredient.environmentScore, isLeftGrab);
                DataLogger.Log("ScoreUI", "Showing EnvScore " + ingredient.environmentScore + " for " + ingredient.name);
            }
        }
    }

    public void Hide(bool isLeftGrab)
    {
        if (isLeftGrab)
        {
            leftCircles.SetActive(false);
        }
        else
        {
            rightCircles.SetActive(false);
        }
        DataLogger.Log("ScoreUI", "Hiding UI");
    }

    private void disableAll(bool isLeftSide)
    {
        List<GameObject> backgrounds = rightBackgrounds;
        if (isLeftSide) backgrounds = leftBackgrounds;
        foreach (GameObject inner in backgrounds)
        {
            inner.SetActive(false);
        }
    }
}
