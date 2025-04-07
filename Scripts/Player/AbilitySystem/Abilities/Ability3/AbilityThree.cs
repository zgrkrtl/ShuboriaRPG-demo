using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Ability", menuName = "Abilities/AbilityThree")]

public class AbilityThree : AbilitySO
{
    
    public override void ActivateAbility(Transform caster)
    {
        Animator animator = caster.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("AbilityThree");
        }
        else
        {
            Debug.LogWarning("No Animator found on caster!");
        }
    }

    public override void SpawnProjectile(Transform spawnPoint, Vector3 targetPosition)
    {
        if (ParticlePrefab == null) return;
        
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Instantiate(ParticlePrefab, targetPosition, Quaternion.identity,player);
    }
}
