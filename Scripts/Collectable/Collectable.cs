using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static Action<string> OnCollect;
    
    [SerializeField] private string collectableName;
    
    private void OnTriggerEnter(Collider other)
    {
        OnCollect?.Invoke(collectableName);
        Destroy(gameObject, 0.2f);
    }
}
