using System;
using TMPro;
using UnityEngine;

public class GlobeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private CharacterStatsManager characterStatsManager;

    private void Start()
    {
        PlayerHealthAndManaManager.OnHealthManaChanged += OnHealthManaChanged;
        
        healthText.text = $"<color=#FF4C4C>Health:</color> " +
                          $"<color=#FF0000>{(int)characterStatsManager.MaxHealth}</color>" +
                          $"<color=#FFFFFF>/</color>" +
                          $"<color=#8B0000>{(int)characterStatsManager.MaxHealth}</color>";

        manaText.text = $"<color=#4682B4>Mana:</color> " +
                        $"<color=#1E90FF>{(int)characterStatsManager.MaxMana}</color>" +
                        $"<color=#FFFFFF>/</color>" +
                        $"<color=#00008B>{(int)characterStatsManager.MaxMana}</color>";
    }

    private void OnHealthManaChanged(float health, float mana)
    {
        if (Mathf.Approximately(health, characterStatsManager.MaxHealth))
        {
            healthText.text = $"<color=#FF4C4C>Health:</color> " +
                              $"<color=#8B0000>{(int)health}</color>" +
                              $"<color=#FFFFFF>/</color>" +
                              $"<color=#8B0000>{(int)characterStatsManager.MaxHealth}</color>";
        }
        else
        {
            healthText.text = $"<color=#FF4C4C>Health:</color> " +
                              $"<color=#FF0000>{(int)health}</color>" +
                              $"<color=#FFFFFF>/</color>" +
                              $"<color=#8B0000>{(int)characterStatsManager.MaxHealth}</color>";
        }

        if (Mathf.Approximately(mana, characterStatsManager.MaxMana))
        {
            manaText.text = $"<color=#4682B4>Mana:</color> " +
                            $"<color=#00008B>{(int)mana}</color>" +
                            $"<color=#FFFFFF>/</color>" +
                            $"<color=#00008B>{(int)characterStatsManager.MaxMana}</color>";  
        }
        else
        {
            manaText.text = $"<color=#4682B4>Mana:</color> " +
                            $"<color=#1E90FF>{(int)mana}</color>" +
                            $"<color=#FFFFFF>/</color>" +
                            $"<color=#00008B>{(int)characterStatsManager.MaxMana}</color>";  
        }
        
    }
}