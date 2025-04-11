using UnityEngine;

public class Quest
{
   // static information and state information
   public QuestInfoSO info;
   public QuestState state;
   
   private int currentQuestStepIndex;

   public Quest(QuestInfoSO questInfo)
   {
      info = questInfo;
      state = QuestState.REQUIREMENTS_NOT_MET;
      currentQuestStepIndex = 0;
   }

   public void MoveToNextStep()
   {
      currentQuestStepIndex++;
   }

   public bool CurrentStepExists()
   {
      return currentQuestStepIndex < info.questStepPrefabs.Length;
   }

   public void InstantiateCurrentQuestStep(Transform parentTransform)
   {
      GameObject questStepPrefab = GetCurrentQuestStepGameObject();
      if (questStepPrefab != null)
      {
         QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
         questStep.InitializeQuestStep(info.id);
      }
   }

   private GameObject GetCurrentQuestStepGameObject()
   {
      GameObject questStepPrefab = null;
      if (CurrentStepExists())
      {
         questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
      }
      else
      {
         Debug.LogError("Quest Step Prefab Not Found: QuestId {info.id} stepIndex {currentQuestStepIndex}.");
      }
      return questStepPrefab;
   }
}
