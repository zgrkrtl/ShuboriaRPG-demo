using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityManager : MonoBehaviour
{
    // Events for UI or other systems to listen to ability usage and cooldown updates
    public static Action<AbilitySO, float> OnCooldownUpdated;
    public static Action<AbilitySO> OnAbilityUsed;

    // Assigned through inspector
    [SerializeField] private AbilitySO attackAbilitySO;
    [SerializeField] private AbilitySO ability1SO;
    [SerializeField] private AbilitySO ability2SO;
    [SerializeField] private AbilitySO ability3SO;
    
    [FormerlySerializedAs("spawnPoint")]
    [SerializeField] private Transform fireballSpawnPoint;

    [SerializeField] private InputManager inputManager;

    private bool isAttacking = false;
    private Vector3 lastMouseScreenPosition;
    private Animator animator;

    // Track cooldown time left for each ability
    private Dictionary<AbilitySO, float> cooldownTimers = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitializeCooldowns();
    }

    // Initializes cooldown values to zero for all abilities
    private void InitializeCooldowns()
    {
        AbilitySO[] allAbilities = { attackAbilitySO, ability1SO, ability2SO, ability3SO };
        foreach (var ability in allAbilities)
            cooldownTimers[ability] = 0f;
    }

    private void Update()
    {
        animator.SetBool("IsAttacking", isAttacking);

        // Update cooldowns over time
        var keys = new List<AbilitySO>(cooldownTimers.Keys);
        foreach (var ability in keys)
        {
            if (cooldownTimers[ability] > 0)
                cooldownTimers[ability] -= Time.deltaTime;
        }
    }

    // Helper to check if an ability is still on cooldown
    private bool IsOnCooldown(AbilitySO ability) => cooldownTimers[ability] > 0;

    // Start the cooldown and notify any listeners
    private void SetCooldown(AbilitySO ability)
    {
        cooldownTimers[ability] = ability.CooldownTime;
        OnCooldownUpdated?.Invoke(ability, ability.CooldownTime);
        OnAbilityUsed?.Invoke(ability);
    }

    private void OnEnable() => BindInputs(true);
    private void OnDisable() => BindInputs(false);

    // Toggle input event bindings
    private void BindInputs(bool subscribe)
    {
        if (inputManager == null) return;

        if (subscribe)
        {
            inputManager.OnAttack += HandleAttack;
            inputManager.OnAbility1 += HandleAbilityOne;
            inputManager.OnAbility2 += HandleAbilityTwo;
            inputManager.OnAbility3 += HandleAbilityThree;
        }
        else
        {
            inputManager.OnAttack -= HandleAttack;
            inputManager.OnAbility1 -= HandleAbilityOne;
            inputManager.OnAbility2 -= HandleAbilityTwo;
            inputManager.OnAbility3 -= HandleAbilityThree;
        }
    }

    // Attack action triggered via input
    private void HandleAttack(Transform target)
    {
        if (IsOnCooldown(attackAbilitySO) || target == null || isAttacking) return;

        isAttacking = true;
        AttackAbility.target = target;

        // Smoothly rotate towards the target before attacking
        StartCoroutine(SmoothRotateTowards(target.position, () =>
        {
            attackAbilitySO.ActivateAbility(transform);
            SetCooldown(attackAbilitySO);
        }));
    }

    private void HandleAbilityOne(Vector3 targetPos) => HandleAbility(targetPos, ability1SO);
    private void HandleAbilityTwo(Vector3 targetPos) => HandleAbility(targetPos, ability2SO);

    // Generalized handler for ranged/targeted abilities
    private void HandleAbility(Vector3 targetPos, AbilitySO ability)
    {
        if (IsOnCooldown(ability) || isAttacking) return;

        isAttacking = true;
        lastMouseScreenPosition = targetPos;

        StartCoroutine(SmoothRotateTowards(targetPos, () =>
        {
            ability.ActivateAbility(transform);
            SetCooldown(ability);
        }));
    }

    // Ability 3 might be instant/self-cast, no target position needed
    private void HandleAbilityThree()
    {
        if (IsOnCooldown(ability3SO) || isAttacking) return;

        isAttacking = true;
        ability3SO.ActivateAbility(transform);
        SetCooldown(ability3SO);
    }

    // Rotates character toward a world position, then runs a callback (e.g., fire ability)
    private IEnumerator SmoothRotateTowards(Vector3 targetPos, Action onComplete = null)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            Quaternion startRot = transform.rotation;
            float elapsed = 0f;
            float duration = 0.15f;

            while (elapsed < duration)
            {
                transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRot;
        }

        // Allow attack to proceed
        onComplete?.Invoke();
    }

    // Called from animation events (optional: combine these with ability parameter)
    public void SpawnProjectile() => attackAbilitySO.SpawnProjectile(fireballSpawnPoint, lastMouseScreenPosition);
    public void SpawnProjectileAbilityOne() => ability1SO.SpawnProjectile(transform, lastMouseScreenPosition);
    public void SpawnProjectileAbilityTwo() => ability2SO.SpawnProjectile(transform, lastMouseScreenPosition);
    public void SpawnEffectAbilityThree() => ability3SO.SpawnProjectile(transform, transform.position);

    public void OnAttackAnimationEnd() => isAttacking = false;
    public bool GetIsAttacking() => isAttacking;
}
