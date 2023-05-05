using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveLoadManager
{
    public static SaveData CurrentsaveData = new SaveData();

    public const string saveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";
    public static bool Save()
    {
        var dir = Application.persistentDataPath + saveDirectory;



        return true;
    }
}
