using System.IO;
using UnityEngine;

public class MandatoryDataSaveManager
{
    private static string filePath = Application.persistentDataPath + "/mandatoryData.json";

    public static void Save(MandatoryData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Data saved successfully at: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving data: " + ex.Message);
        }
    }

    public static MandatoryData Load()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<MandatoryData>(json);
            }
            else
            {
                Debug.LogWarning("Save file not found. Creating new MandatoryData instance.");
                return new MandatoryData();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading data: " + ex.Message);
            return new MandatoryData();
        }
    }
}