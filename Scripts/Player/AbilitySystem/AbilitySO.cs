using System.Collections.Generic;
using UnityEngine;


public abstract class AbilitySO : ScriptableObject
{
  [SerializeField] private string abilityName;
  [SerializeField] private string description;
  [SerializeField] private Sprite icon;
  [SerializeField] private GameObject particlePrefab;
  [SerializeField] private int cooldownTime;
  [SerializeField] private float manaCost;
  
  
  public string AbilityName { get => abilityName; set => abilityName = value; }
  public string Description { get => description; set => description = value; }
  public Sprite Icon { get => icon; set => icon = value; }
  public GameObject ParticlePrefab { get => particlePrefab; set => particlePrefab = value; }
  public int CooldownTime { get => cooldownTime; set => cooldownTime = value; }
  public float ManaCost { get => manaCost; set => manaCost = value; }
  public abstract void ActivateAbility(Transform caster);
  
  public abstract void SpawnProjectile(Transform spawnPoint, Vector3 targetPosition);
}
