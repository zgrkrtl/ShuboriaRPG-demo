using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  
  private Dictionary<string, Quest> questMap;
  private int currentPlayerLevel;
  
  private MandatoryData mandatoryData;
  private List<QuestData> loadedData;
  private void Awake()
  {
    questMap = createQuestMap();
  }

  private void OnEnable()
  {
    QuestEventManager.instance.questEvents.onStartQuest += StartQuest;
    QuestEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
    QuestEventManager.instance.questEvents.onFinishQuest += FinishQuest;
    QuestEventManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    
    GameplayManager.instance.OnExperienceAndLevelChange += OnExperienceAndLevelChange;
  }
  

  private void OnDisable()
  {
    QuestEventManager.instance.questEvents.onStartQuest -= StartQuest;
    QuestEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
    QuestEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
    QuestEventManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    
    GameplayManager.instance.OnExperienceAndLevelChange -= OnExperienceAndLevelChange;
  }
  
  private void Start()
  {
    // load mandatory data into game
    mandatoryData = MandatoryDataSaveManager.Load();
    
    // Inform UI of current quest states
    foreach (Quest quest in questMap.Values)
    {
      // initialize any loaded quest steps
      if (quest.state == QuestState.IN_PROGRESS)
      {
        quest.InstantiateCurrentQuestStep(transform);
      }
      QuestEventManager.instance.questEvents.QuestStateChange(quest);
    }
    
    currentPlayerLevel = mandatoryData.level;
  }

  private void Update()
  {
    // loop through all quests
    foreach (Quest quest in questMap.Values)
    {
      // if meets requirements switch to CAN_START state
      if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
      {
        ChangeQuestState(quest.info.id,QuestState.CAN_START);
      }
    }
  }
  
  private Dictionary<string, Quest> createQuestMap()
  {
    // Loads all SO
    QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
    
    // Create a quest map
    Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
    Dictionary<string, QuestData> savedQuestData = QuestDataSaveManager.Load();

    foreach (QuestInfoSO questInfo in allQuests)
    {
      if (idToQuestMap.ContainsKey(questInfo.id))
      {
        Debug.LogError("duplicated id found when creating quest map: " + questInfo.id);
      }

      if (savedQuestData.TryGetValue(questInfo.id, out QuestData questData))
      {
        idToQuestMap.Add(questInfo.id, new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates));
      }
      else
      {
        idToQuestMap.Add(questInfo.id, new Quest(questInfo));
      }
    }

    return idToQuestMap;
  }
  
  private void OnExperienceAndLevelChange(int level, float exp)
  {
    currentPlayerLevel = level;
  }
  
  private bool CheckRequirementsMet(Quest quest)
  {
    // start true and prove to be false
    bool meetsRequirements = true;

    // check player level requirements
    if (currentPlayerLevel < quest.info.levelRequirement)
    {
      meetsRequirements = false;
    }

    // check quest prerequisites for completion
    foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
    {
      if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
      {
        meetsRequirements = false;
        break;
      }
    }

    return meetsRequirements;
  }

  private void ChangeQuestState(string id, QuestState state)
  {
    Quest quest = GetQuestById(id);
    quest.state = state;
    QuestEventManager.instance.questEvents.QuestStateChange(quest);
  }

  private void StartQuest(string id)
  {
    Quest quest = GetQuestById(id);
    quest.InstantiateCurrentQuestStep(transform);
    ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
  }
  private void AdvanceQuest(string id)
  {
    Quest quest = GetQuestById(id);
    
    // move on to next step
    quest.MoveToNextStep();
    
    // if there are more steps, instantiate next one 
    if (quest.CurrentStepExists())
    {
      quest.InstantiateCurrentQuestStep(transform);
    }
    // if there are no more steps, we can switch to CAN_FINISH state
    else
    {
      ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
    }
  }
  private void FinishQuest(string id)
  {
    Quest quest = GetQuestById(id);
    ClaimRewards(quest);
    ChangeQuestState(quest.info.id, QuestState.FINISHED);
  }

  private void ClaimRewards(Quest quest)
  {
    // Implement gain experience
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
  
  private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
  {
    Quest quest = GetQuestById(id);
    quest.StoreQuestStepState(questStepState,stepIndex);
    ChangeQuestState(quest.info.id, quest.state);
  }

  public void ResetAllQuests()
  {
    foreach (var quest in questMap.Values)
    {
      quest.Reset();
      QuestEventManager.instance.questEvents.QuestStateChange(quest);
    }
    QuestDataSaveManager.Save(questMap);
  }
  private void OnApplicationQuit()
  {
    QuestDataSaveManager.Save(questMap);
  }
}
