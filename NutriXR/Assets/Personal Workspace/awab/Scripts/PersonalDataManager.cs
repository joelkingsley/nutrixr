using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalDataManager : MonoBehaviour
{
    string _saveFile;

    void Awake()
    {
        _saveFile = Application.persistentDataPath + "/gamedata.json";
    }

    void Start()
    {
        ReadFile();
    }

    public void ReadFile()
    {
        if (File.Exists(_saveFile))
        {
            string fileContents = File.ReadAllText(_saveFile);
            Debug.Log(fileContents);
        }
    }

    public void WriteFile(string jsonString)
    {
        Debug.Log("writing...");
        Debug.Log(jsonString);
        File.WriteAllText(_saveFile, jsonString);

        Debug.Log("done writing");
    }
}
