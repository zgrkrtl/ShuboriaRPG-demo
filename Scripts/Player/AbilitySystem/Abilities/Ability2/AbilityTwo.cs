using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Ability", menuName = "Abilities/AbilityTwo")]
public class AbilityTwo : AbilitySO
{
    public override void ActivateAbility(Transform caster)
    {

        Animator animator = caster.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("AbilityTwo");
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
        GameObject projectileInstance = Instantiate(ParticlePrefab, targetPosition, Quaternion.identity);
    }
}
