using Newtonsoft.Json;
using System.IO;
using System.Xml;
using UnityEngine;

public class DataFileWriter
{
    string dirPath = "";
    string fileName = "gameData.json";
    string fullPath;

    public DataFileWriter(string directoryPath)
    {
        dirPath = directoryPath;
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        fullPath = Path.Combine(dirPath, fileName);
        if (!System.IO.File.Exists(fullPath))
        {
            System.IO.File.Create(fullPath);
        }
    }

    public void Save(GameData data) {
        Debug.Log(data.booleans.ToString());
        string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None);
        Debug.Log(json);
        try
        {
            File.Delete(fullPath);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate)) {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(json);
                }
            }
            Debug.Log($"Game data saved to {fullPath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }
    public GameData Load() {
        GameData loaded = null;
        try
        {
            /*loaded = File.OpenRead(fullPath) != null ?

                JsonUtility.FromJson<GameData>(File.ReadAllText(fullPath)) :
                null;
            */
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    loaded = JsonConvert.DeserializeObject<GameData>(reader.ReadToEnd());
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
        return loaded;
    }
}
