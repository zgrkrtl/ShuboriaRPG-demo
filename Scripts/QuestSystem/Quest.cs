using UnityEngine;

public class Quest
{
   // static information and state information
   public QuestInfoSO info;
   public QuestState state;
   
   private int currentQuestStepIndex;
   private QuestStepState[] questStepStates;

   public Quest(QuestInfoSO questInfo)
   {
      info = questInfo;
      state = QuestState.REQUIREMENTS_NOT_MET;
      currentQuestStepIndex = 0;
      questStepStates = new QuestStepState[info.questStepPrefabs.Length];
      for (int i = 0; i < questStepStates.Length; i++)
      {
         questStepStates[i] = new QuestStepState();
      }
   }

   public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestStepIndex,
      QuestStepState[] questStepStates)
   {
      info = questInfo;
      state = questState;
      this.currentQuestStepIndex = currentQuestStepIndex;
      this.questStepStates = questStepStates;

      if (this.questStepStates.Length != info.questStepPrefabs.Length)
      {
         Debug.LogWarning("Quest step prefabs and quest step states are not the same length. + " + info.id);
      }
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
         questStep.InitializeQuestStep(info.id,currentQuestStepIndex,questStepStates[currentQuestStepIndex].state);
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
         Debug.LogError($"Quest Step Prefab Not Found: QuestId {info.id} stepIndex {currentQuestStepIndex}.");
      }
      return questStepPrefab;
   }

   public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
   {
      if (stepIndex < questStepStates.Length)
      {
         questStepStates[stepIndex].state = questStepState.state;
      }
      else
      {
         Debug.LogWarning($"Tried to access quest step data, but stepIndex was out of range: Quest Id: {info.id}, Step Index: {stepIndex}.");
      }
   }

   public QuestData GetQuestData()
   {
      return new QuestData(state, currentQuestStepIndex, questStepStates);
   }
   
   public void Reset()
   {
      state = QuestState.REQUIREMENTS_NOT_MET;
      currentQuestStepIndex = 0;

      for (int i = 0; i < questStepStates.Length; i++)
      {
         questStepStates[i].state = null;
      }
   }
}
