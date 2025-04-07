using System;
using UnityEngine;

public class StatsTreeComponent : MonoBehaviour
{
    public Action OnUnlockedAction;   
    
    [SerializeField] private CharacterStatsManager characterStatsManager;
    [SerializeField] private StatTreeManager statTreeManager;

    public enum StatType
    {
        Vitality,
        Dexterity,
        Intelligence
    }
    
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private StatType statType;
    [SerializeField] private GameObject dependentObject;

    public bool isUnlocked = false;
    public bool isDefaultComponent = false;

    private void Awake()
    {
        statTreeManager.OnReset += OnReset;
        
        if (isDefaultComponent || isUnlocked)
        {
            isUnlocked = true;
            selectedImage.SetActive(false);
        }
    }

    private void Start()
    {
        if (isDefaultComponent || isUnlocked)
        {
            isUnlocked = true;
            selectedImage.SetActive(false);
        }
    }

    private void OnReset()
    {
        if (isDefaultComponent) return;
             selectedImage.SetActive(!isUnlocked);
    }

    public void OnButtonClick()
    {
        if (statTreeManager.abilityPoints <= 0) return;

        if (isUnlocked)
        {
            
        }
        else
        {
            if (dependentObject != null)
            {
                if (dependentObject.GetComponent<StatsTreeComponent>() == null)
                {
                    if (dependentObject.GetComponent<UniqueStatsTreeComponent>().isUnlocked == false) return;
                }
                else
                {
                    if (dependentObject.GetComponent<StatsTreeComponent>().isUnlocked == false) return;
                }
            }
        
            if (selectedImage != null)
            {
                selectedImage.SetActive(false);
            }
        
            ApplyStatIncrease();
            statTreeManager.DecreaseAbilityPoints();
            isUnlocked = true;
            OnUnlockedAction?.Invoke();  
        }
        
    }
    
    private void ApplyStatIncrease()
    {
        switch (statType)
        {
            case StatType.Vitality:
                characterStatsManager.Vitality = Mathf.Min(100, characterStatsManager.Vitality + 1);
                break;
            case StatType.Dexterity:
                characterStatsManager.Dexterity = Mathf.Min(100, characterStatsManager.Dexterity + 1);
                break;
            case StatType.Intelligence:
                characterStatsManager.Intelligence = Mathf.Min(100, characterStatsManager.Intelligence + 1);
                break;
        }
        
        characterStatsManager.ReCalculateEverything();
    }

    
}
