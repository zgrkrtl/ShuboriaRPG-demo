using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Ability", menuName = "Abilities/AbilityOne")]

public class AbilityOne : AbilitySO
{
    public override void ActivateAbility(Transform caster)
    {
        
        Animator animator = caster.GetComponent<Animator>();

        if (animator != null)
        {
           animator.SetTrigger("AbilityOne");
        }
        else
        {
            Debug.LogWarning("No Animator found on caster!");
        }
    }

    public override void SpawnProjectile(Transform spawnPoint, Vector3 targetPosition)
    {
        if (ParticlePrefab == null)
        {
            return;
        }
        Instantiate(ParticlePrefab, targetPosition, Quaternion.identity);
    }
}
