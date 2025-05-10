using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO",order = 1)]
public class QuestInfoSO : ScriptableObject
{
   [field: SerializeField] public string id { get; private set; }
   
   [Header("General")]
   public string displayName;
   
   [Header("Requirements")]
   public int levelRequirement;
   public QuestInfoSO[] questPrerequisites;
   
   [Header("Steps")] 
   public GameObject[] questStepPrefabs;

   [Header("Rewards")]
   public int goldReward;
   public int experienceReward;

   
   // ensure the id is always same as SO name
   private void OnValidate()
   {
      #if UNITY_EDITOR
      id = name;
      UnityEditor.EditorUtility.SetDirty(this);
      #endif
   }
}
