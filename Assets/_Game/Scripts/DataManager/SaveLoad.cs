using Newtonsoft.Json;
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
    //In progress vvv
    /*public static string ObfuscateJSON(object objToObfuscate)
    {
        string obfJson = JsonConvert.SerializeObject(objToObfuscate);
        return obfJson;
    }
    public static object DeobfuscateJSON(string objToDeobfuscate)
    {
        object obj = JsonConvert.DeserializeObject(objToDeobfuscate);
        return obj;
    }*/
}

