using UnityEngine;

public class RedExplosion : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float baseDamage = 50f;
    
    private void Update()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage);
            }
        }
    }
}
