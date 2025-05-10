using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class QuestDataSaveManager
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "quest_data.json");

    public static void Save(Dictionary<string, Quest> questMap)
    {
        QuestSaveWrapper wrapper = new QuestSaveWrapper();

        foreach (var pair in questMap)
        {
            wrapper.quests.Add(new QuestSaveEntry(pair.Key, pair.Value.GetQuestData()));
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static Dictionary<string, QuestData> Load()
    {
        if (!File.Exists(savePath)) return new Dictionary<string, QuestData>();

        string json = File.ReadAllText(savePath);
        QuestSaveWrapper wrapper = JsonUtility.FromJson<QuestSaveWrapper>(json);

        Dictionary<string, QuestData> data = new Dictionary<string, QuestData>();
        foreach (var entry in wrapper.quests)
        {
            data[entry.questId] = entry.questData;
        }

        return data;
    }
}

[System.Serializable]
public class QuestSaveWrapper
{
    public List<QuestSaveEntry> quests = new List<QuestSaveEntry>();
}

[System.Serializable]
public class QuestSaveEntry
{
    public string questId;
    public QuestData questData;

    public QuestSaveEntry(string id, QuestData data)
    {
        questId = id;
        questData = data;
    }
}
