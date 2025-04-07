using System;
using UnityEngine;

public class UniqueStatsTreeComponent : MonoBehaviour
{
    public Action OnUnlockedAction;
    
    [SerializeField] private CharacterStatsManager characterStatsManager;
    [SerializeField] private StatTreeManager statTreeManager;


    [SerializeField] private GameObject selectedImage;
    [SerializeField] private GameObject dependentObject;
    
    public bool isUnlocked = false;

    private void Awake()
    {
        if (selectedImage.activeSelf == false || isUnlocked)
        {
            isUnlocked = true;
            selectedImage.SetActive(false);
        }
    }

    private void Start()
    {
        statTreeManager.OnReset += OnReset;
        
        if (selectedImage.activeSelf == false || isUnlocked)
        {
            isUnlocked = true;
            selectedImage.SetActive(false);
        }
    }
    
    private void OnReset()
    {
        selectedImage.SetActive(!isUnlocked);
    }
    
    public void OnButtonClick()
    {
        if (isUnlocked || statTreeManager.abilityPoints <= 0 || 
            dependentObject != null && dependentObject.GetComponent<StatsTreeComponent>().isUnlocked == false) return;
        
        
        if (selectedImage != null)
        {
            selectedImage.SetActive(false);
        }
        
        ApplyStatIncrease();
        statTreeManager.DecreaseAbilityPoints();

        isUnlocked = !isUnlocked;
        OnUnlockedAction?.Invoke();
    }

    private void ApplyStatIncrease()
    {
        // Special stat increase here later
    }
}
