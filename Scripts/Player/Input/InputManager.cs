using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public event Action<Vector2> OnMove;
    public event Action<Transform> OnAttack;
    public event Action<Vector3> OnAbility1;
    public event Action<Vector3> OnAbility2;
    
    public event Action OnAbility3;
    public event Action ToggleCharacterInfo;
    public event Action OnInteract;


    [SerializeField] private AbilitySO attackAbilitySO;
    [SerializeField] private AbilitySO ability1SO;
    [SerializeField] private AbilitySO ability2SO;
    [SerializeField] private AbilitySO ability3SO;

    public Transform attackTarget;
    
    [SerializeField] PlayerHealthAndManaManager playerHealthAndManaManager;
    
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
    

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Attack.performed += ctx => 
        {
            if (attackTarget == null || playerHealthAndManaManager.GetCurrentMana() < attackAbilitySO.ManaCost)
            {
                Debug.Log("No target assigned or not enough mana");
                return;
            }

            OnAttack?.Invoke(attackTarget);
        };
        
        playerInputActions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        playerInputActions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
        playerInputActions.Player.Ability1.performed += ctx =>
        {
            if (attackTarget == null || playerHealthAndManaManager.GetCurrentMana() < ability1SO.ManaCost )
            {
                Debug.Log("No target assigned or not enough mana");
                return;
            }

            OnAbility1?.Invoke(attackTarget.position);
        };
        
        playerInputActions.Player.Ability2.performed += ctx =>
        {
            if (attackTarget == null || playerHealthAndManaManager.GetCurrentMana() < ability2SO.ManaCost)
            {
                Debug.Log("No target assigned or not enough mana");
                return;
            }

            OnAbility2?.Invoke(attackTarget.position);
        };


        playerInputActions.Player.Ability3.performed += ctx =>
        {
            if (playerHealthAndManaManager.GetCurrentMana() < ability3SO.ManaCost)
            {
                Debug.Log("No target assigned or not enough mana");
                return;
            }
            OnAbility3?.Invoke();
        };
        
        playerInputActions.Player.ToggleCharacterInfo.performed += ctx => ToggleCharacterInfo?.Invoke();
        playerInputActions.Player.Interact.performed += ctx => OnInteract?.Invoke();


        PlayerHealthAndManaManager.OnDeath += PlayerHealthManager_OnDeath;
    }

    private void PlayerHealthManager_OnDeath()
    {
        playerInputActions.Disable();
    }
}