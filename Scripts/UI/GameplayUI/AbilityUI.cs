using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownTimeNumericOverlay;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private AbilitySO ability;

    private Coroutine cooldownCoroutine;

    private void Awake()
    {
        abilityIcon.sprite = ability.Icon;
        cooldownOverlay.fillAmount = 0; // Ensure it's not visible at the start
        cooldownTimeNumericOverlay.text = "";
    }

    public void StartCooldown(float cooldown)
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        
        StartCoroutine(CooldownTimer(cooldown));
    }

    

    private IEnumerator CooldownTimer(float cooldownDuration)
    {
        float timeRemaining = cooldownDuration;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            cooldownOverlay.fillAmount = timeRemaining / cooldownDuration;
            cooldownTimeNumericOverlay.text = timeRemaining.ToString("0.0");
            if(timeRemaining <= 0) cooldownTimeNumericOverlay.text = "";
            yield return null;
        }

        cooldownOverlay.fillAmount = 0; // Ensure it's fully reset
    }
    

    public AbilitySO GetAbilitySO() => ability;
}