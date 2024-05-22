using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Org.BouncyCastle.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NutriScoreUI : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private GameObject circles;
    [SerializeField] private GameObject text;
    private List<char> colors = new List<char>() { 'A', 'B', 'C', 'D', 'E'};
    [SerializeField] private List<GameObject> backgrounds;
    private Dictionary<char, GameObject> colorDict = new Dictionary<char, GameObject>(); //0:A, 1:B, 2:C, 3:D, 4:E

    private void Update()
    {
        transform.position = hand.position + new Vector3(0.07f, 0.07f, 0);
        Vector3 eyeDirection = eyeCenter.position - transform.position;
        transform.localRotation = Quaternion.LookRotation(-eyeDirection.normalized, transform.up);
        transform.localRotation.Set(transform.rotation.x+90, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void Start()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colorDict.Add(colors[i], backgrounds[i]);
        }
    }

    public void setNutriScore(char score)
    {
        Debug.Log("set nutri score: " + score);

        if (score == 'Z')
        {
            circles.SetActive(false);
            text.SetActive(false);
            return;
        }

        circles.SetActive(true);
        text.SetActive(true);
        disableAll();
        colorDict[score].SetActive(true);
        text.GetComponent<TextMeshProUGUI>().text = Char.ToString(score);
    }

    private void disableAll()
    {
        foreach (GameObject inner in backgrounds)
        {
            inner.SetActive(false);
        }
    }
}
