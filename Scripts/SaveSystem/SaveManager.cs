using System.IO;
using UnityEngine;
    
public static class SaveManager
{
    private static string filePath = Application.persistentDataPath + "/gameData.json";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }
    
 
    public static GameData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(json);
            return new GameData(loadedData.Stats);
        }
        else
        {
            return null;
        }
    }
}