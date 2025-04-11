using System;
using TMPro;
using UnityEngine;

public class StatTreeManager : MonoBehaviour
{
    public Action OnReset;
    public Action<int> OnAbilityPointsChanged;
    
    [SerializeField] private GameObject[] components;
    [SerializeField] private CharacterStatsManager characterStatsManager;
    [SerializeField] private TextMeshProUGUI abilityPointsText;

    public int abilityPoints;
    private SkillTreeData skillTreeData;
    private MandatoryData mandatoryData;
    private void Start()
    {
        GameplayManager.instance.OnLevelUp += () => IncreaseAbilityPoints();
        
        mandatoryData = MandatoryDataSaveManager.Load();
        abilityPointsText.text = mandatoryData.abilityPoints.ToString();
        abilityPoints = mandatoryData.abilityPoints; 
        
        foreach (GameObject component in components)
        {
            if(component.GetComponent<StatsTreeComponent>())
            {
                component.GetComponent<StatsTreeComponent>().OnUnlockedAction += RewriteData;
            }
            else
            {
                component.GetComponent<UniqueStatsTreeComponent>().OnUnlockedAction += RewriteData;
            }
        }
        
        skillTreeData = SkillTreeSaveManager.Load();
        
        for (int i = 0; i < components.Length; i++)
        {
            if(components[i].GetComponent<StatsTreeComponent>())
            {
                if (components[i].GetComponent<StatsTreeComponent>().isDefaultComponent) continue;
                components[i].GetComponent<StatsTreeComponent>().isUnlocked = skillTreeData.UnlockedStats[i];
            }
            else
            {
                components[i].GetComponent<UniqueStatsTreeComponent>().isUnlocked = skillTreeData.UnlockedStats[i];
            }
        }
    }
    

    public void RewriteData()
    {
        for (int i = 0; i < components.Length; i++)
        {
            if(components[i].GetComponent<StatsTreeComponent>())
            {
                if (components[i].GetComponent<StatsTreeComponent>().isDefaultComponent) continue;
                skillTreeData.UnlockedStats[i] = components[i].GetComponent<StatsTreeComponent>().isUnlocked;
            }
            else
            {
                skillTreeData.UnlockedStats[i] = components[i].GetComponent<UniqueStatsTreeComponent>().isUnlocked;
            }
        }
        
        SkillTreeSaveManager.Save(skillTreeData);
    }

  
    public void ResetStatTree()
    {
        int abilitiesThatAreUnlocked = 0;
        for (int i = 0; i < components.Length; i++)
        {
            
            if(components[i].GetComponent<StatsTreeComponent>())
            {
                if (components[i].GetComponent<StatsTreeComponent>().isDefaultComponent) continue;
                if(components[i].GetComponent<StatsTreeComponent>().isUnlocked) abilitiesThatAreUnlocked++;
                
                components[i].GetComponent<StatsTreeComponent>().isUnlocked = false;
                skillTreeData.UnlockedStats[i] = false;
            }
            else
            {
                if(components[i].GetComponent<UniqueStatsTreeComponent>().isUnlocked) abilitiesThatAreUnlocked++;

                components[i].GetComponent<UniqueStatsTreeComponent>().isUnlocked = false;
                skillTreeData.UnlockedStats[i] = false;
            }
        }
        
        // Reset Stats
        characterStatsManager.Vitality = 1;
        characterStatsManager.Dexterity = 1;
        characterStatsManager.Intelligence = 1;
        characterStatsManager.ReCalculateEverything();
        
        
        IncreaseAbilityPoints(abilitiesThatAreUnlocked);
        
        SkillTreeSaveManager.Save(skillTreeData);
        OnReset?.Invoke();
    }

    public void IncreaseAbilityPoints(int value = 1)
    {
        abilityPoints = Mathf.Min(100, abilityPoints + value);
        mandatoryData.abilityPoints = abilityPoints;
        abilityPointsText.text = abilityPoints.ToString();

        OnAbilityPointsChanged?.Invoke(abilityPoints);
    }

    public void DecreaseAbilityPoints()
    {
        abilityPoints = Mathf.Max(0, abilityPoints - 1);
        mandatoryData.abilityPoints = abilityPoints;
        abilityPointsText.text = abilityPoints.ToString();
        
        OnAbilityPointsChanged?.Invoke(abilityPoints);
    }
    
}
