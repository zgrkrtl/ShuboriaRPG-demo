using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public  class EnemyBase : MonoBehaviour, IHoverable
{
    public Action<float> OnDamageTaken;
    public static Action<float> OnEnemyDeath;
    
    [SerializeField] protected EnemySO enemyData;
    [SerializeField] protected GameObject selectionIndicator;
    [SerializeField] private CharacterStatsManager characterStatsManager;
    [SerializeField] private GameObject damagePopUpPrefab;
    [SerializeField] private GameObject experiencePopUpPrefab;
    [SerializeField] private GameObject healthBar;

    // essentials
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private Collider enemyCollider;
    private float currentHealth;
    private bool isHovered;
    private bool isDead = false;
    private bool isAttacking = false;
    public bool IsHovered
    {
        get => isHovered;
        set => isHovered = value;
    }
    
    // these control patrolling 
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private  float sightRange, attackRange, rotationSpeed, patrolRadius;
   
    
    // some interior control mechanism and necessary bools
    private Vector3 walkPoint, startPoint;
    private bool playerInSightRange, playerInAttackRange, walkPointSet;
    private bool IsWaiting = false;
    private bool IsGoingBackToBase = false;
    private bool isRightRotation = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider>();
        
        currentHealth = enemyData.MaxHealth;
        startPoint = transform.position;
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }
    }

    private void Start()
    {
        selectionIndicator.SetActive(false);
        startPoint = transform.position;
    }

    private void Update()
    {
        if (isDead) return;
        
        animator.SetFloat("Speed", agent.velocity.magnitude);
        
        // check if player is in sight range or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // patrolling mechanism
        if (!IsGoingBackToBase)
        {
            if (!playerInAttackRange && !playerInSightRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange && !IsOutOfPatrol()) ChasePlayer();
        }

        if (IsOutOfPatrol() && !IsGoingBackToBase)
        {
            StartCoroutine(BackToBase(5f));
        }
    }
    
    // enemy patrols or walks around random in a determined radius
    private void Patrolling()
    {
        if (!walkPointSet && !IsWaiting) SearchWalkPoint();
        
        if (walkPointSet)
        {
                Quaternion targetRotation = Quaternion.LookRotation(walkPoint - agent.transform.position);
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                agent.SetDestination(walkPoint);
                StartCoroutine(WaitBeforeNextPoint(3f)); 
            
                walkPointSet = false;
        }
        
    }
    
    
    // If out of patrol radius or on exactly the last pixel it goes back to base while chasing player
    private IEnumerator BackToBase(float waitTime)
    {
        agent.isStopped = true;  
        IsGoingBackToBase = true;

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            agent.isStopped = true;
            IsGoingBackToBase = false;        
            yield break;
        }

        Quaternion targetRotation = Quaternion.LookRotation(startPoint - transform.position);
    
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            if (isDead)
            {
                agent.isStopped = true;
                IsGoingBackToBase = false;          
                yield break;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        if (isDead)
        {
            agent.isStopped = true;
            IsGoingBackToBase = false;           
            yield break;
        }

        agent.isStopped = false;
        agent.SetDestination(startPoint);

        float timer = 0f;
        while (timer < waitTime)
        {
            if (isDead)
            {
                agent.isStopped = true;
                IsGoingBackToBase = false;              
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        IsGoingBackToBase = false;
    }
    
    private IEnumerator WaitBeforeNextPoint(float waitTime)
    {
        IsWaiting = true;
        yield return new WaitForSeconds(waitTime);
        IsWaiting = false;
    }

    private void SearchWalkPoint()
    {
        float randomAngle = Random.Range(0f, 360f); 
        float randomDistance = Random.Range(0f, patrolRadius); 

        float randomX = startPoint.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float randomZ = startPoint.z + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

        walkPoint = new Vector3(randomX, transform.position.y, randomZ);

        if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }
    
    private void ChasePlayer()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 2f && !IsOutOfPatrol()) 
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        
            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            if (PlayerHealthAndManaManager.isDead) return;
            
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (!isAttacking) 
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
            }
        }
    }

    // This attack is an event method for attack animation it is called into "Attack"
    public void Attack()
    {
        if (enemyData.HitEffectPrefab != null)
        {
            Quaternion finalRotation;

            if (isRightRotation)
            {
                finalRotation = transform.rotation * Quaternion.Euler(0, 15f, 0);
            }
            else
            {
                finalRotation = transform.rotation * Quaternion.Euler(0, -15f, 0);
            }

            GameObject hitEffect = Instantiate(enemyData.HitEffectPrefab, transform.position, finalRotation);

            Destroy(hitEffect, enemyData.HitEffectPrefab.GetComponent<ParticleSystem>().main.duration);

            isRightRotation = !isRightRotation;
        }
        
        float attackRange = 2f; 
        Vector3 attackPosition = transform.position + transform.forward * (attackRange * 0.5f); 

        Collider[] hitObjects = Physics.OverlapSphere(attackPosition, 1f); 
        foreach (Collider hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
              hit.GetComponent<PlayerHealthAndManaManager>().TakeDamage(enemyData.Damage);
            }
        }
        isAttacking = false; 
    }

    private bool IsOutOfPatrol()
    {
        return Vector3.Distance(transform.position, startPoint) > patrolRadius;
    }

    // IHoverable Interface overriden function
    public void SetHoverState(bool isHovered)
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(isHovered);
            this.isHovered = isHovered;
        }
    }

    // Damage taking logic
    public void TakeDamage(float baseDamage)
    {
        animator.SetTrigger("HitReact");
        
        float damageTaken = (float)(baseDamage + characterStatsManager.AbilityDamage * 0.5);
        
        currentHealth -= damageTaken;
       
        if(damagePopUpPrefab != null ) ShowFloatingTextDamage(damageTaken);
        
        OnDamageTaken?.Invoke(currentHealth/enemyData.MaxHealth);

        agent.isStopped = true;
        agent.isStopped = false;
        
        if (currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(enemyData.ExperiencePoints);
            agent.isStopped = true;
            if(experiencePopUpPrefab != null) ShowFloatingTextExperience(enemyData.ExperiencePoints);
            isDead = true;
            healthBar.SetActive(false);
            enemyCollider.enabled = false;
            animator.SetTrigger("Die");
        }
    }

    private void ShowFloatingTextDamage(float damageTaken)
    {
        var floatingText = Instantiate(damagePopUpPrefab, transform.position + Vector3.up * 3f, Quaternion.identity, transform);
        floatingText.GetComponent<TextMeshPro>().text = damageTaken.ToString("0");
    }
    
    private void ShowFloatingTextExperience(float experiencePoints)
    {
        var floatingText = Instantiate(experiencePopUpPrefab, transform.position + Vector3.forward * 5f, Quaternion.identity, transform);
        floatingText.GetComponent<TextMeshPro>().text = "+" +experiencePoints.ToString("0") + "EXP"; 
    }

    // This is called in "Death" animation events
    
    public void DestroyGameObjectAfterDeath() 
    {
        Destroy(gameObject,1f);
    }
    
}
