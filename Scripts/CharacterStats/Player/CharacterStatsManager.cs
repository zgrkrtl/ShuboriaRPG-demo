using System;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public Action OnRecalculation;

    // Base values
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseMaxMana = 100f;
    [SerializeField] private float baseArmor = 5f;
    [SerializeField] private float baseAbilityDamage = 5f;
    [SerializeField] private float baseHealthRegen = 1f;
    [SerializeField] private float baseManaRegen = 1f;

    private GameData gameData;
    
    // Primary attributes -- These are changed by secondary attributes

    public float MaxHealth { get; private set; }
    public float MaxMana { get; private set; }
    public float Armor { get; private set; }
    public float AbilityDamage { get; private set; }
    public float HealthRegen { get; private set; }
    public float ManaRegen { get; private set; }

    // Secondary attributes -- These change primary attributes
    
    public float Vitality { get; set; }
    public float Intelligence { get; set; }
    public float Dexterity { get; set; }

    private void Start()
    {
        gameData = SaveManager.Load();
        AssignStats(gameData);  
        ReCalculateEverything(); 
    }   

    // Assign only vit-int-dex
    private void AssignStats(GameData gameData)
    {
        if (gameData == null || gameData.Stats == null) return;

        Vitality = gameData.Stats.Vitality;
        Intelligence = gameData.Stats.Intelligence;
        Dexterity = gameData.Stats.Dexterity;

        ReCalculateEverything();  
    }

    // self explanatory
    public void ReCalculateEverything()
    {
        CalculateMaxHealth();
        CalculateMaxMana();
        CalculateArmor();
        CalculateAbilityDamage();
        CalculateHealthRegen();
        CalculateManaRegen();
        
        OnRecalculation?.Invoke();
        SaveStats(); 
    }

    private void CalculateMaxHealth() => MaxHealth = baseMaxHealth + Vitality * 5;
    private void CalculateMaxMana() => MaxMana = baseMaxMana + Intelligence * 5;
    private void CalculateArmor() => Armor = (float)(baseArmor + Vitality * 1 + Dexterity * 0.5);
    private void CalculateAbilityDamage() => AbilityDamage = baseAbilityDamage + Vitality * 1 + Dexterity * 1 + Intelligence * 5;
    private void CalculateHealthRegen() => HealthRegen = (float)(baseHealthRegen + Vitality * 0.1);
    private void CalculateManaRegen() => ManaRegen = (float)(baseManaRegen + Intelligence * 0.1);
    
    
    // save everything to json file
    private void SaveStats()
    {
        if (gameData == null) return;

        gameData.Stats.MaxHealth = MaxHealth;
        gameData.Stats.MaxMana = MaxMana;
        gameData.Stats.Armor = Armor;
        gameData.Stats.AbilityDamage = AbilityDamage;
        gameData.Stats.HealthRegen = HealthRegen;
        gameData.Stats.ManaRegen = ManaRegen;
        gameData.Stats.Vitality = Vitality;
        gameData.Stats.Intelligence = Intelligence;
        gameData.Stats.Dexterity = Dexterity;

        SaveManager.Save(gameData);
    }
}
