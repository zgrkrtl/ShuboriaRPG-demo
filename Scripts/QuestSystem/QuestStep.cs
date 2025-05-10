using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void InitializeQuestStep(string questId, int stepIndex,string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            QuestEventManager.instance.questEvents.AdvanceQuest(questId);
            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState)
    {
        QuestEventManager.instance.questEvents.OnQuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string state);
}
