using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
   public Action<int, float> OnExperienceAndLevelChange;  
   public Action OnLevelUp;
   
   [SerializeField] StatTreeManager statTreeManager;

   
   private int currentLevel;
   private float experiencePoints = 0f;
   private float currentExperiencePercentage;
   private int abilityPoints;
   
   private MandatoryData mandatoryData;
   
   private void Start()
   {
      // binding necessary delegates
      
      statTreeManager.OnAbilityPointsChanged += OnAbilityPointsChanged;
      EnemyBase.OnEnemyDeath += OnEnemyDeath;
      
      // load mandatory data into game
      mandatoryData = MandatoryDataSaveManager.Load();
      abilityPoints = mandatoryData.abilityPoints;
      currentLevel = mandatoryData.level;
      experiencePoints = mandatoryData.experiencePoints;
      currentExperiencePercentage = experiencePoints / ExperienceRequiredForLevel(currentLevel+1);
   }

   private void OnAbilityPointsChanged(int value)
   {
      mandatoryData.abilityPoints = value;
      MandatoryDataSaveManager.Save(mandatoryData);
   }

   private void OnEnemyDeath(float experience)
   {
      experiencePoints += experience;
      CalculateLevel();
   }

   private void CalculateLevel()
   {
      while (experiencePoints >= ExperienceRequiredForLevel(currentLevel+1))
      {
         experiencePoints -= ExperienceRequiredForLevel(currentLevel+1);
         
         currentLevel++;
         abilityPoints++;
         OnLevelUp?.Invoke();
         
         currentExperiencePercentage = experiencePoints / ExperienceRequiredForLevel(currentLevel+1); 
         OnExperienceAndLevelChange?.Invoke(currentLevel, currentExperiencePercentage);
      }
      currentExperiencePercentage = experiencePoints / ExperienceRequiredForLevel(currentLevel+1); 
      
     // Saving data into json file
      mandatoryData.level = currentLevel;
      mandatoryData.experiencePoints = experiencePoints;
      mandatoryData.abilityPoints = abilityPoints;
      MandatoryDataSaveManager.Save(mandatoryData);
      
      OnExperienceAndLevelChange?.Invoke(currentLevel, currentExperiencePercentage);
   }

   // Formula for required xp for LEVEL
   // RequiredXP = BaseXP X (LEVEL)^GrowthFactor 
   public float ExperienceRequiredForLevel(int level)
   {
      float baseXP = 100f;
      float growthFactor = 1.5f;
      return baseXP * Mathf.Pow(level, growthFactor);
   }
   
   // When application quit preserve player's last position
    private void OnApplicationQuit()
    {
        mandatoryData.position = new SerializableVector3(FindAnyObjectByType<PlayerController>().transform.position);
        MandatoryDataSaveManager.Save(mandatoryData);
    }
}
