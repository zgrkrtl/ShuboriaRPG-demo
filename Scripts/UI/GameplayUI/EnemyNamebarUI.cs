using UnityEngine;

public class EnemyNamebarUI : MonoBehaviour
{
    [SerializeField] private Camera cam;  
    
    private void Update()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.transform.forward);
    }
    
}
