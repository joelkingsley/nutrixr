using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class DataLogger
{
    private static bool newSessionStarted = false;
    private static readonly string path = Path.Combine(Application.persistentDataPath, "LogData.txt");

    public static string ID;
    public static string GOAL;

    public static bool IsFirstRun = true;

    public static void Load()
    {
        if (!File.Exists(path))
        {
            //Create and close the file
            File.Create(path).Close();
        }

        if (IsPersonalDataAvailabe())
        {
            IsFirstRun = false;
            foreach (string line in File.ReadAllLines(path))
            {
                if (line.StartsWith("[PERSONAL]"))
                {
                    ID = line.Split("$")[1];
                    GOAL = line.Split("$")[2];
                }
            }
        }
        else
        {
            IsFirstRun = true;
        }
    }

    public static void LogPersonal(string id, string goal)
    {
        if (!IsPersonalDataAvailabe())
        {
            ID = id;
            GOAL = goal;
            Log("PERSONAL", "$" + id + "$"  + goal);
        }
    }

    public static bool IsPersonalDataAvailabe()
    {
        if (File.Exists(path) && File.ReadAllLines(path).Any(line => line.StartsWith("[PERSONAL]")))
        {
            return true;
        }
        return false;
    }

    public static void DeleteLogFile()
    {
        Log("DataLogger", "Deleting Log file");
        File.Delete(path);
        Load();
    }

    public static void Log(string sender, string msg)
    {
        //First message that gets logged will be started after some space to quickly detect new sessions.
        if (!newSessionStarted) {
            string newSessionString = "\n\n##########";
            using (StreamWriter sr = new StreamWriter(path, true)) {
                sr.WriteLine(newSessionString);
            }
            newSessionStarted = true;
        }


        DateTime localDate = DateTime.Now;
        string to_write = "[" + sender + "][" + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff") + "]: " + msg;

        using (StreamWriter sr = new StreamWriter(path, true)) {
            sr.WriteLine(to_write);
            sr.Close();
        }
    }
}
