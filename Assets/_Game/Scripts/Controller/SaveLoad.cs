using System.IO;
using UnityEngine;

public class SaveLoad
{
    public static string SaveToJSON(object objToSave, string filePath, string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        if (!File.Exists(filePath))
            File.Create(filePath);

        string json = JsonUtility.ToJson(objToSave);

        return json;
    }
    public static void LoadFromJSON(string json, object objToLoad)
    {
        JsonUtility.FromJsonOverwrite(json, objToLoad);
    }
}

