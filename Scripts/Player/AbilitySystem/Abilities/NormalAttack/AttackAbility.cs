using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Ability", menuName = "Abilities/Attack Ability")]
public class AttackAbility : AbilitySO
{
    public static Transform target;
    public override void ActivateAbility(Transform caster)
    {

        Animator animator = caster.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            Debug.LogWarning("No Animator found on caster!");
        }
    }

    public override void SpawnProjectile(Transform spawnPoint, Vector3 targetPosition)
    {
        if (ParticlePrefab == null || spawnPoint == null)
        {
            return;
        }

        // Calculate the direction to the target
        Vector3 direction = (targetPosition - spawnPoint.position).normalized;

        // Ensure the projectile is facing the target when spawned
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Instantiate the fireball with the correct rotation
        GameObject projectileInstance = Instantiate(ParticlePrefab, spawnPoint.position, lookRotation);

        FireBall fireBall = projectileInstance.GetComponent<FireBall>();
        if (fireBall != null)
        {
            fireBall.SetTarget(target);
        }
    }
    
}