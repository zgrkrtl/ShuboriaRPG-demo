using System;
using UnityEngine;

public class QuestEventManager : MonoBehaviour
{
    public static QuestEventManager instance { get; private set; }
    
    public QuestEvents questEvents;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;
        
        questEvents = new QuestEvents();
    }
}