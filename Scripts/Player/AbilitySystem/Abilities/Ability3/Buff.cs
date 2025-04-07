using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    private void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
