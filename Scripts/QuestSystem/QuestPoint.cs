using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;
    
    [Header("InputManager")]
    [SerializeField] private InputManager inputManager;

    
    
    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        QuestEventManager.instance.questEvents.onQuestStateChange += OnQuestStateChange;
        inputManager.OnInteract += InputManagerOnInteract;
        
    }

    private void InputManagerOnInteract()
    {
        if (!playerIsNear) return;
        
        QuestEventManager.instance.questEvents.StartQuest(questId);
    }

    private void OnDisable()
    {
        QuestEventManager.instance.questEvents.onQuestStateChange -= OnQuestStateChange;
        inputManager.OnInteract -= InputManagerOnInteract;

    }

    private void OnQuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id == questId)
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsNear = false;
        }
    }
}
