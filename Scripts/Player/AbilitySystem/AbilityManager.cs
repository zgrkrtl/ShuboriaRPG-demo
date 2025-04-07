using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityManager : MonoBehaviour
{
    public static Action<AbilitySO, float> OnCooldownUpdated;
    public static Action<AbilitySO> OnAbilityUsed;
    
    // Abilities
    
    [SerializeField] private AbilitySO attackAbilitySO;
    [SerializeField] private AbilitySO ability1SO;
    [SerializeField] private AbilitySO ability2SO;
    [SerializeField] private AbilitySO ability3SO;
    
    [FormerlySerializedAs("spawnPoint")] [SerializeField] private Transform fireballSpawnPoint; 
    [SerializeField] private InputManager inputManager;
    
    private bool isAttacking = false;
    private Vector3 lastMouseScreenPosition;
    private Animator animator;

    private Dictionary<AbilitySO, float> cooldownTimers = new Dictionary<AbilitySO, float>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cooldownTimers[attackAbilitySO] = 0f;
        cooldownTimers[ability1SO] = 0f;
        cooldownTimers[ability2SO] = 0f;
        cooldownTimers[ability3SO] = 0f;
    }

    private void Update()
    {
        animator.SetBool("IsAttacking",isAttacking);
        List<AbilitySO> keys = new List<AbilitySO>(cooldownTimers.Keys);
        foreach (var ability in keys)
        {
            if (cooldownTimers[ability] > 0)
            {
                cooldownTimers[ability] -= Time.deltaTime;
            }
        }   
    }

    private bool IsOnCooldown(AbilitySO ability)
    {
        return cooldownTimers[ability] > 0;
    }

    private void SetCooldown(AbilitySO ability)
    {
        cooldownTimers[ability] = ability.CooldownTime;
        OnCooldownUpdated?.Invoke(ability, ability.CooldownTime);
        OnAbilityUsed?.Invoke(ability);
    }
    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnAttack += HandleAttack;
            inputManager.OnAbility1 += HandleAbilityOne;
            inputManager.OnAbility2 += HandleAbilityTwo;
            inputManager.OnAbility3 += HandleAbilityThree;
        }
    }
    
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnAttack -= HandleAttack;
            inputManager.OnAbility1 -= HandleAbilityOne;
            inputManager.OnAbility2 -= HandleAbilityTwo;
            inputManager.OnAbility3 -= HandleAbilityThree;
        }
    }
    

    private void HandleAttack(Transform target)
    {
        if (IsOnCooldown(attackAbilitySO) || target == null) return;

        if (!isAttacking)
        {
            isAttacking = true;

            StartCoroutine(SmoothRotateTowards(target));

            attackAbilitySO.ActivateAbility(transform);
            AttackAbility.target = target;
            SetCooldown(attackAbilitySO);
        }
    }

    private IEnumerator SmoothRotateTowards(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0; 

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;
            float rotationDuration = 0.15f; 

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }

            transform.rotation = targetRotation; 
        }
    }
    
    private void HandleAbilityOne(Vector3 targetPosition)
    {
        HandleAbility(targetPosition, ability1SO);
    }

    private void HandleAbilityTwo(Vector3 targetPosition)
    {
        HandleAbility(targetPosition, ability2SO);
    }

    private void HandleAbility(Vector3 targetPosition, AbilitySO ability)
    {
        if (IsOnCooldown(ability)) return;

        if (!isAttacking)
        {
            isAttacking = true;
            lastMouseScreenPosition = targetPosition;

            StartCoroutine(SmoothRotateTowards(targetPosition, () => 
            {
                ability.ActivateAbility(transform);
                SetCooldown(ability);
            }));
        }
    }

    private IEnumerator SmoothRotateTowards(Vector3 targetPosition, Action onComplete)
    {
        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection.y = 0; 

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;
            float rotationDuration = 0.15f;

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            onComplete?.Invoke();
        }
    }
    
    private void HandleAbilityThree()
    {
        if (IsOnCooldown(ability3SO)) return;

        if (!isAttacking)
        {
            ability3SO.ActivateAbility(transform);
            SetCooldown(ability3SO);
        }
    }
    
    
    // These called on events for each ability
    
    public void SpawnProjectile()
    {
        attackAbilitySO.SpawnProjectile(fireballSpawnPoint,lastMouseScreenPosition);
    }
    
    public void SpawnProjectileAbilityOne()
    {
        ability1SO.SpawnProjectile(transform,lastMouseScreenPosition);
    }
    
    public void SpawnProjectileAbilityTwo()
    {
        ability2SO.SpawnProjectile(transform,lastMouseScreenPosition);
    }

    public void SpawnEffectAbilityThree()
    {
        ability3SO.SpawnProjectile(transform,transform.position);
    }
    
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
    
    public bool GetIsAttacking()
    {
        return isAttacking;
    }
}