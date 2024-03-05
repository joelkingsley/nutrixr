using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UI.Toggle;

public class UIHandler : MonoBehaviour
{
    public Button Done;

    public GameObject personalDataUI;

    public GameObject leftHandUI;

    public GameDataManager gameDataManager;

    public TMP_InputField ageInputField;

    public TMP_InputField heightInputField;

    public Toggle toggleFemale;

    public Toggle toggleMale;

    public TMP_Dropdown activityLevel;

    private Basket _basket;

    private RecipeSystem _recipeSystem;
    // Start is called before the first frame update
    void Start()
    {
        _basket = GetComponent<Basket>();
        _recipeSystem = GetComponent<RecipeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (leftHandUI.activeSelf)
            {
                leftHandUI.SetActive(false);
            }
            else
            {
                leftHandUI.SetActive(true);
                _basket.Redraw();
                _recipeSystem.RedrawRecipeUI();
            }
        }
    }

    public void DoneIsClicked()
    {
        personalDataUI.SetActive(false);
        gameDataManager.WriteFile("{"+
                                  $"\"age\":{ageInputField.text}," +
                                  $"\"height\":{heightInputField.text}," +
                                  $"\"female\":{toggleFemale.isOn}," +
                                  $"\"male\":{toggleMale.isOn}," +
                                  $"\"activity\":{activityLevel.value}"+
                                  "}");

    }

}
