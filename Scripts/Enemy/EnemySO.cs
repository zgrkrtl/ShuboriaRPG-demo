using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string enemyName;
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float patrolRadius;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float experiencePoints;

    public string Name => enemyName;
    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public float Speed => speed;
    public float PatrolRadius => patrolRadius;
    public float SightRange => sightRange;
    public float AttackRange => attackRange;
    public float ExperiencePoints => experiencePoints;
    public GameObject HitEffectPrefab => hitEffectPrefab;
}
