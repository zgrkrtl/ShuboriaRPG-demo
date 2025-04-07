using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  private Dictionary<string, Quest> questMap;

  private void Awake()
  {
    questMap = createQuestMap();
  }

  private void OnEnable()
  {
    QuestEventManager.instance.questEvents.onStartQuest += StartQuest;
    QuestEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
    QuestEventManager.instance.questEvents.onFinishQuest += FinishQuest;
  }

  private void OnDisable()
  {
    QuestEventManager.instance.questEvents.onStartQuest -= StartQuest;
    QuestEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
    QuestEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
  }

  private void Start()
  {
    foreach (Quest quest in questMap.Values)
    {
      QuestEventManager.instance.questEvents.QuestStateChange(quest);
    }
  }

  private void StartQuest(string id)
  {
    
  }
  private void AdvanceQuest(string id)
  {
    
  }
  private void FinishQuest(string id)
  {
    
  }
  
  
  private Dictionary<string, Quest> createQuestMap()
  {
    // Loads all SO
    QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
    
    // Create a quest map
    Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
    foreach (QuestInfoSO questInfo in allQuests)
    {
      if (idToQuestMap.ContainsKey(questInfo.id))
      {
        Debug.LogError("duplicated id found when creating quest map: " + questInfo.id);
      }
      idToQuestMap.Add(questInfo.id, new Quest(questInfo));
    }

    return idToQuestMap;
  }

  private Quest GetQuestById(string id)
  {
    Quest quest = questMap[id];
    if (quest == null)
    {
      Debug.LogError("quest not found: " + id);
    }

    return quest;
  }
}
