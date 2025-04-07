using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float baseDamage = 10f;

    private Vector3 moveDirection;

    private Transform target;
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move in the initially stored direction, avoiding sudden redirection
        transform.position += moveDirection * (speed * Time.deltaTime);
    
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        moveDirection = (target.position - transform.position).normalized; // Store the direction on spawn
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hitEffectPrefab != null)
            {
               GameObject hitEffect =  Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
               Destroy(hitEffect, hitEffectPrefab.GetComponent<ParticleSystem>().main.duration);
            }

            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage);
            }
            Destroy(gameObject);
        }
    }
}