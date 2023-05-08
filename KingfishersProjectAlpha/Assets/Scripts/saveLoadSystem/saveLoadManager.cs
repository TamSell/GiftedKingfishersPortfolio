using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class saveLoadManager
{
    public static SaveData CurrentsaveData = new SaveData();

    public const string saveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";
    public static bool SaveGame()
    {
        var dir = Application.persistentDataPath + saveDirectory;

        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(CurrentsaveData, prettyPrint:true);
        File.WriteAllText(dir + FileName, json);

        GUIUtility.systemCopyBuffer = dir;

        return true;
    }

    public static void LoadGame()
    {
        string fullPath = Application.persistentDataPath + saveDirectory + FileName;
        SaveData tempData = new SaveData();

        if(File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            tempData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogError(message:"Save File Doesn't Exist");
        }

        CurrentsaveData = tempData;
    }
}
