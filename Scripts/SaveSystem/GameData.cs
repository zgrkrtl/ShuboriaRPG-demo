using UnityEngine;

[System.Serializable]
public class GameData
{
    public CharacterStats Stats;
    
    public GameData(CharacterStats stats)
    {
        Stats = new CharacterStats(
            stats.MaxHealth, stats.MaxMana, stats.Armor, stats.AbilityDamage,
            stats.HealthRegen, stats.ManaRegen, stats.Vitality,
            stats.Intelligence, stats.Dexterity
        );
    }
}


[System.Serializable]
public class CharacterStats
{
    public float MaxHealth;
    public float MaxMana;
    public float Armor;
    public float AbilityDamage;
    public float HealthRegen;
    public float ManaRegen;
    public float Vitality;
    public float Intelligence;
    public float Dexterity;

    

    public CharacterStats(float maxHealth, float maxMana, float armor, float abilityDamage,
        float healthRegen, float manaRegen, float vitality,
        float intelligence, float dexterity)
    {
        MaxHealth = maxHealth;
        MaxMana = maxMana;
        Armor = armor;
        AbilityDamage = abilityDamage;
        HealthRegen = healthRegen;
        ManaRegen = manaRegen;
        Vitality = vitality;
        Intelligence = intelligence;
        Dexterity = dexterity;
    }
}

