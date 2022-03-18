using System.IO;
using UnityEngine;

class SaveLoad
{
    public static void SaveLevelDatasToJSON(object objToSave, string path)
    {
        string json = JsonUtility.ToJson(objToSave);

        FileStream fileStream = new FileStream(path, FileMode.Create);

        if (!File.Exists(path))
            File.Create(path);
        

    }
    public static void LoadLevelDatasFromJSON(string json, object objToLoad)
    {
        JsonUtility.FromJsonOverwrite(json, objToLoad);
    }
}

