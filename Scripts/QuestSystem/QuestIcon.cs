using System;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;
    
    private void Update()
    { 
        transform.LookAt(Camera.main.transform);
    }

    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementsNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementsNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);
        
        // set the appropriate one to active based on the new state

        switch (newState)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                if(startPoint) {requirementsNotMetToStartIcon.SetActive(true);}
                break;
            case QuestState.CAN_START:
                if(startPoint) {canStartIcon.SetActive(true);}
                break;
            case QuestState.IN_PROGRESS:
                if(finishPoint) {requirementsNotMetToFinishIcon.SetActive(true);}
                break;
            case QuestState.CAN_FINISH:
                if(finishPoint) {canFinishIcon.SetActive(true);}
                break;
            default:
                Debug.Log("Quest Finished or Unknown quest state" + newState);
                break;
        }
    }
}
