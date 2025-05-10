using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")] 
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    [SerializeField] private GameObject interactableText;
    [SerializeField] private GameObject interactableSelect;
    
    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;
    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void Start()
    {
        QuestEventManager.instance.questEvents.onQuestStateChange += OnQuestStateChange;
        InputManager.instance.OnInteract += InputManagerOnInteract;
    }
    

    private void OnDisable()
    {
        QuestEventManager.instance.questEvents.onQuestStateChange -= OnQuestStateChange;
        InputManager.instance.OnInteract -= InputManagerOnInteract;
    }

    
    private void InputManagerOnInteract()
    {
        if (!playerIsNear) return;
        
        // start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            QuestEventManager.instance.questEvents.StartQuest(questId);
        }else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            QuestEventManager.instance.questEvents.FinishQuest(questId);
        }
    }

    
    private void OnQuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id == questId)
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState,startPoint,finishPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SetEverything(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SetEverything(false);
        }
    }

    private void SetEverything(bool state)
    {
        playerIsNear = state;
        interactableText.SetActive(state);
        interactableSelect.SetActive(state);
    }
}
