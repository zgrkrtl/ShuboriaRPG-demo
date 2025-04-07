using System.IO;
using UnityEngine;

public static class SkillTreeSaveManager
{
    private static string filePath = Application.persistentDataPath + "/skillTreeData.json";

    public static void Save(SkillTreeData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static SkillTreeData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SkillTreeData>(json);
        }
        else
        {
            return new SkillTreeData(); 
        }
    }
}
