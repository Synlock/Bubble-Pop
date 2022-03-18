using System.IO;
using UnityEngine;

public class SaveLoad
{
    public static string SaveLevelDatasToJSON(object objToSave, string path)
    {
        string json = JsonUtility.ToJson(objToSave);

        FileStream fileStream = new FileStream(path, FileMode.Create);

        if (!File.Exists(path))
            File.Create(path);

        return json;
    }
    public static void LoadLevelDatasFromJSON(string json, object objToLoad)
    {
        JsonUtility.FromJsonOverwrite(json, objToLoad);
    }
}

