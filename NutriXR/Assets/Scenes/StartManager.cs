using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using Oculus.Avatar2;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject scanButton;
    [SerializeField] private GameObject joinButton;

    [SerializeField] private GameObject studySetupUI;
    [SerializeField] private GameObject studyRejoinUI;
    [SerializeField] private TextMeshProUGUI IDField;
    [SerializeField] private TextMeshProUGUI GoalField;

    //Fields used for the rejoin UI in the second iteration
    [SerializeField] private TextMeshProUGUI FixIDField;
    [SerializeField] private TextMeshProUGUI FixGoalField;

    // Start is called before the first frame update
    private void Start()
    {
#if UNITY_EDITOR
        //This app runs in the Unity Editor

        //NetworkManager.singleton.StartClient();
        //NetworkManager.singleton.StartHost();
        NetworkManager.singleton.StartServer();
#endif
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void StartMenuClicked()
    {
        DataLogger.Load();
        if (!DataLogger.IsPersonalDataAvailabe())
        {
            studySetupUI.SetActive(true);
            DataLogger.Log("StartManager", "Personal data is not available. Promoting StudySetupUI.");
        }
        else
        {
            FixIDField.SetText(DataLogger.ID);
            FixGoalField.SetText(DataLogger.GOAL);
            studyRejoinUI.SetActive(true);
            DataLogger.Log("StartManager", "Personal data is available. Promoting StudyRejoinUI.");
        }
    }

    public void StudySetupUI_JoinClicked()
    {
        DataLogger.Log("StartManager", "Logging Personal Data:");
        DataLogger.LogPersonal(IDField.text, GoalField.text);
        NetworkManager.singleton.StartClient();
    }

    public void StudyRejoinUI_JoinClicked()
    {
        DataLogger.Log("StartManager", "Reusing Personal Data. Do not log again.");
        NetworkManager.singleton.StartClient();
    }

    public void DeleteLogFile()
    {
        DataLogger.DeleteLogFile();
    }
}
