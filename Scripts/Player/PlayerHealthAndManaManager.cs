using System;
using TMPro;
using UnityEngine;

public class PlayerHealthAndManaManager : MonoBehaviour
{
   public static Action OnDeath;
   
   // send percentages
   public static Action<float> OnHealthChanged;
   public static Action<float> OnManaChanged;
   
   // send actual values
   public static Action<float, float> OnHealthManaChanged;

   
   [SerializeField] private CharacterStatsManager characterStatsManager;
   [SerializeField] private GameObject damagePopUpPrefab;

   
   public static bool isDead = false;
   private float currentHealth;
   private float currentMana;
   
   private Animator animator;

   private void Awake()
   {
      AbilityManager.OnAbilityUsed += OnAbilityUsedHandleMana;
   }

   private void Start()
   {
      animator = GetComponent<Animator>();
      currentHealth = characterStatsManager.MaxHealth;
      currentMana = characterStatsManager.MaxMana;
      
      OnHealthChanged?.Invoke(currentHealth/characterStatsManager.MaxHealth);
      OnManaChanged?.Invoke(currentMana/characterStatsManager.MaxMana);
      
      OnHealthManaChanged?.Invoke(currentHealth,currentMana);
      
      InvokeRepeating(nameof(RegenerateStats), 1f, 1f);
   }

   private void OnAbilityUsedHandleMana(AbilitySO ability)
   {
      currentMana = Mathf.Max(0, currentMana - ability.ManaCost);
      
      OnHealthManaChanged?.Invoke(currentHealth,currentMana);
      OnManaChanged?.Invoke(currentMana/characterStatsManager.MaxMana);
   }
   
   public void TakeDamage(float damage)
   {
      if (isDead) return;

      float mitigatedDamage = CalculateMitigatedDamage(damage);
        
      // pop up floating damage text here 
      if(damagePopUpPrefab != null) ShowFloatingTextDamage(mitigatedDamage);
      
      currentHealth = Mathf.Max(currentHealth - mitigatedDamage, 0);
      
      OnHealthManaChanged?.Invoke(currentHealth,currentMana);
      OnHealthChanged?.Invoke(currentHealth/characterStatsManager.MaxHealth);
      
      animator.SetTrigger("HitReact");
      
      if (currentHealth <= 0f)
      {
         Die();
      }
   }
   
   private void ShowFloatingTextDamage(float damageTaken)
   {
      var floatingText = Instantiate(damagePopUpPrefab, transform.position + Vector3.up * 3f, Quaternion.identity, transform);
      floatingText.GetComponent<TextMeshPro>().text = damageTaken.ToString("0");
   }
   private float CalculateMitigatedDamage(float rawDamage)
   {
      float armor = characterStatsManager.Armor;

      float damageReductionFactor = 100f / (100f + armor);
      return rawDamage * damageReductionFactor;
   }
   
   
   private void RegenerateStats()
   {
      if (isDead) return; 

      bool healthChanged = currentHealth < characterStatsManager.MaxHealth;
      bool manaChanged = currentMana < characterStatsManager.MaxMana;

      currentHealth = Mathf.Min(currentHealth + characterStatsManager.HealthRegen, characterStatsManager.MaxHealth);
      currentMana = Mathf.Min(currentMana + characterStatsManager.ManaRegen, characterStatsManager.MaxMana);
      
      if (healthChanged)
      {
         OnHealthChanged?.Invoke(currentHealth / characterStatsManager.MaxHealth);
         OnHealthManaChanged?.Invoke(currentHealth, currentMana);
      }

      if (manaChanged)
      {
         OnManaChanged?.Invoke(currentMana / characterStatsManager.MaxMana);
         OnHealthManaChanged?.Invoke(currentHealth, currentMana);
      }

      
   }
   private void Die()
   {
      animator.SetTrigger("Die");
      OnDeath?.Invoke();
      isDead = true;
   }
 
   public float GetCurrentMana()
   {
      return currentMana;
   }

   public float GetCurrentHealth()
   {
      return currentHealth;
   }
}
