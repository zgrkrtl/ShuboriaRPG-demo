using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            
            // TODO - ADVANCE THE QUEST FORWARD 
            
            Destroy(gameObject);
            
        }
    }
}
