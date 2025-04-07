using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject healthGlobe;
    [SerializeField] private GameObject manaGlobe;
    [SerializeField] private AbilityUI attackAbilityUI;
    [SerializeField] private AbilityUI ability1UI;
    [SerializeField] private AbilityUI ability2UI;
    [SerializeField] private AbilityUI ability3UI;


    private Dictionary<AbilitySO, AbilityUI> abilityUIDictionary = new Dictionary<AbilitySO, AbilityUI>();

    private Image healthGlobeFillImage;
    private Image manaGlobeFillImage;
    
    private void Awake()
    {
        // Subscribe to cooldown updates
        AbilityManager.OnCooldownUpdated += OnCooldownUpdated;
        PlayerHealthAndManaManager.OnHealthChanged += UpdateHealthGlobe;
        PlayerHealthAndManaManager.OnManaChanged += UpdateManaGlobe;
        
        
        // Map AbilitySO to UI elements
        abilityUIDictionary[attackAbilityUI.GetAbilitySO()] = attackAbilityUI;
        abilityUIDictionary[ability1UI.GetAbilitySO()] = ability1UI;
        abilityUIDictionary[ability2UI.GetAbilitySO()] = ability2UI;
        abilityUIDictionary[ability3UI.GetAbilitySO()] = ability3UI;
        
        if (healthGlobe == null || manaGlobe == null)
        {
            Debug.LogError("Globe is not assigned in the inspector!", this);
            return;
        }

        healthGlobeFillImage = healthGlobe.GetComponent<Image>();
        manaGlobeFillImage = manaGlobe.GetComponent<Image>();

        if (healthGlobeFillImage == null || manaGlobeFillImage == null)
        {
            Debug.LogError("No Image component found", this);
        }
        
    }

    private void OnCooldownUpdated(AbilitySO ability, float cooldown)
    {
        if (abilityUIDictionary.TryGetValue(ability, out AbilityUI abilityUI))
        {
            abilityUI.StartCooldown(cooldown);
        }
    }

    private void OnEnable()
    {
        PlayerHealthAndManaManager.OnHealthChanged += UpdateHealthGlobe;
        PlayerHealthAndManaManager.OnManaChanged += UpdateManaGlobe;

    }

    private void OnDisable()
    {
        PlayerHealthAndManaManager.OnHealthChanged -= UpdateHealthGlobe;
        PlayerHealthAndManaManager.OnManaChanged -= UpdateManaGlobe;

    }

    private void UpdateHealthGlobe(float percent)
    {
        healthGlobeFillImage.fillAmount = percent;
    }

    private void UpdateManaGlobe(float percent)
    {
        manaGlobeFillImage.fillAmount = percent;
    }
    
    
}