using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyBase;
    [SerializeField] private Camera cam;  
    [SerializeField] private Image healthFill;  
    private void Awake()
    {
        healthFill.fillAmount = 1;
        enemyBase.OnDamageTaken += OnDamageTaken;
    }
    
    private void Update()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.transform.forward);
    }
    
    private void OnDamageTaken(float percent)
    {
        Debug.Log(percent);
        healthFill.fillAmount = percent;
    }
}