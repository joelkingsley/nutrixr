using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Org.BouncyCastle.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnvScoreUI : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private GameObject circles;
    [SerializeField] private List<GameObject> backgrounds; // 0 (green), 1 (yellow), 2 (red)

    void Update()
    {
        transform.position = hand.position + new Vector3(0.07f, 0.07f, 0);
        Vector3 eyeDirection = eyeCenter.position - transform.position;
        transform.localRotation = Quaternion.LookRotation(-eyeDirection.normalized, transform.up);
        transform.localRotation.Set(transform.rotation.x+90, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void ShowEnvScore(int score)
    {
        // scores are 0 (green), 1 (yellow), 2 (red)

        circles.SetActive(true);
        disableAll();
        backgrounds[score].SetActive(true);

        Debug.Log("Set Env Score: " + score);
    }

    public void HideScore()
    {
        circles.SetActive(false);

        Debug.Log("Hide Env Score");
    }

    private void disableAll()
    {
        foreach (GameObject inner in backgrounds)
        {
            inner.SetActive(false);
        }
    }
}
