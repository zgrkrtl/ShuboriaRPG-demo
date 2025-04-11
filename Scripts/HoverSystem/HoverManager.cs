using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // For new Input System

public class HoverManager : MonoBehaviour
{
    [SerializeField] private LayerMask hoverableLayers;

    public static HoverManager Instance { get; private set; }

    public IHoverable lastHovered = null;
    private float hoverCheckCooldown = 0.1f;
    private float nextHoverCheckTime = 0f;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void FixedUpdate()
    {
        if (Time.time >= nextHoverCheckTime)
        {
            DetectMouseHover();
            nextHoverCheckTime = Time.time + hoverCheckCooldown; // Check every 0.1s
        }
    }
    
    // Hover manager actually does not just control hovering its also setting target for our player
    private void DetectMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hoverableLayers)) 
        {
            IHoverable hoverable = hit.collider.GetComponent<IHoverable>();
            if (hoverable == lastHovered) return; 

            if (lastHovered != null) lastHovered.SetHoverState(false);

            hoverable.SetHoverState(true);
            lastHovered = hoverable;
            
            // set target
            InputManager.instance.SetAttackTarget(hit.collider.transform);
            return;
        }

        if (lastHovered != null)
        {
            lastHovered.SetHoverState(false);
            lastHovered = null;
        }
        
        // set target
        InputManager.instance.SetAttackTarget(null);
    }
    
}