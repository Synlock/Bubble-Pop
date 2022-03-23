using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
    public static void SaveBinary(string dirPath, string fileName, string stringToWrite)
    {
        string fullPath = $"{dirPath}/{fileName}";

        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
        {
            if (!File.Exists(fullPath))
                File.Create(fullPath);

            using (BinaryWriter writer = new BinaryWriter(fileStream, Encoding.UTF8, false))
            {
                writer.Write(stringToWrite);
            }
        }
    }
    public static string LoadBinary(string filePath)
    {
        if (File.Exists(filePath))
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    string readJson = binaryReader.ReadString();
                    return readJson;
                }
            }
        return "";
    }
    public static string EncodeToB64(string stringToEncode)
    {
        string read = stringToEncode;
        byte[] bytes = Encoding.UTF8.GetBytes(read);
        string b64 = Convert.ToBase64String(bytes);
        return b64;
    }
    public static string DecodeFromB64(string stringToDecode)
    {
        byte[] b64 = Convert.FromBase64String(stringToDecode);
        string decoded = Encoding.UTF8.GetString(b64);
        return decoded;
    }
    public static void SaveAndConvertData(object objToSave, string fileName, string dirPath)
    {
        string fullPath = dirPath + fileName;
        FileStream stream;

        if (!File.Exists(fullPath))
        {
            stream = File.Create(fullPath);
            stream.Close();
        }

        string json = SaveToJSON(objToSave, fullPath, dirPath);
        string encodedJson = EncodeToB64(json);

        File.WriteAllText(fullPath, encodedJson);
    }
    public static void ConvertAndLoadData(object objToLoad, string fileName, string dirPath)
    {
        string fullPath = dirPath + fileName;
        FileStream stream;

        if (!File.Exists(fullPath))
        {
            stream = File.Create(fullPath);
            stream.Close();
        }

        string encodedJson = File.ReadAllText(fullPath);
        string decodedJson = DecodeFromB64(encodedJson);

        LoadFromJSON(decodedJson, objToLoad);
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

