using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UI.Toggle;

public class UIHandler : MonoBehaviour
{
    public Button Done;

    public GameObject uiGameObject;

    public GameDataManager gameDataManager;

    public TMP_InputField ageInputField;

    public TMP_InputField heightInputField;

    public Toggle toggleFemale;

    public Toggle toggleMale;

    public TMP_Dropdown activityLevel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoneIsClicked()
    {
        uiGameObject.SetActive(false);
        gameDataManager.WriteFile("{"+
                                  $"\"age\":{ageInputField.text}," +
                                  $"\"height\":{heightInputField.text}," +
                                  $"\"female\":{toggleFemale.isOn}," +
                                  $"\"male\":{toggleMale.isOn}," +
                                  $"\"activity\":{activityLevel.value}"+
                                  "}");

    }
}
