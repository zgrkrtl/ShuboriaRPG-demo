using TMPro;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private CharacterStatsManager characterStatsManager;

    [SerializeField] private TextMeshProUGUI vitalityText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    
    
    private void Start()
    {
        UpdateStatUI();
        characterStatsManager.OnRecalculation += UpdateStatUI;
    }

    private void OnDestroy()
    {
        characterStatsManager.OnRecalculation -= UpdateStatUI;
    }

    private void UpdateStatUI()
    {
        vitalityText.text = characterStatsManager.Vitality.ToString();
        intelligenceText.text = characterStatsManager.Intelligence.ToString();
        dexterityText.text = characterStatsManager.Dexterity.ToString();
    }

}
