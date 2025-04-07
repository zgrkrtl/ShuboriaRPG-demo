using System;
using TMPro;
using UnityEngine;

public class CharacterInfoUI : MonoBehaviour
{
     [SerializeField] private CharacterStatsManager characterStatsManager;

    [Header("Stat Texts")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI abilityDamageText;
    [SerializeField] private TextMeshProUGUI healthRegenText;
    [SerializeField] private TextMeshProUGUI manaRegenText;
    
    [Header("Secondary Attributes")]
    [SerializeField] private TextMeshProUGUI vitalityText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI dexterityText;

    private void Start()
    {
        UpdateStatUI();
        characterStatsManager.OnRecalculation += UpdateStatUI;
    }

    private void OnDestroy()
    {
        characterStatsManager.OnRecalculation -= UpdateStatUI;
    }

    private void UpdateStatUI()
    {
        healthText.text = characterStatsManager.MaxHealth.ToString();
        manaText.text = characterStatsManager.MaxMana.ToString();
        armorText.text = characterStatsManager.Armor.ToString();
        abilityDamageText.text = characterStatsManager.AbilityDamage.ToString();
        healthRegenText.text = characterStatsManager.HealthRegen.ToString();
        manaRegenText.text = characterStatsManager.ManaRegen.ToString();

        vitalityText.text = characterStatsManager.Vitality.ToString();
        intelligenceText.text = characterStatsManager.Intelligence.ToString();
        dexterityText.text = characterStatsManager.Dexterity.ToString();

    }

    public void IncreaseVitality()
    {
        Debug.Log("IncreaseVitality");
        characterStatsManager.Vitality = Mathf.Min(100, characterStatsManager.Vitality + 1);
        characterStatsManager.ReCalculateEverything();
    }

    public void IncreaseIntelligence()
    {
        characterStatsManager.Intelligence = Mathf.Min(100, characterStatsManager.Intelligence + 1);
        characterStatsManager.ReCalculateEverything();
    }

    public void IncreaseDexterity()
    {
        characterStatsManager.Dexterity = Mathf.Min(100, characterStatsManager.Dexterity + 1);
        characterStatsManager.ReCalculateEverything();
    }

    public void DecreaseVitality()
    {
        characterStatsManager.Vitality = Mathf.Max(0, characterStatsManager.Vitality - 1);
        characterStatsManager.ReCalculateEverything();
    }

    public void DecreaseIntelligence()
    {
        characterStatsManager.Intelligence = Mathf.Max(0, characterStatsManager.Intelligence - 1);
        characterStatsManager.ReCalculateEverything();
    }

    public void DecreaseDexterity()
    {
        characterStatsManager.Dexterity = Mathf.Max(0, characterStatsManager.Dexterity - 1);
        characterStatsManager.ReCalculateEverything();
    }

}