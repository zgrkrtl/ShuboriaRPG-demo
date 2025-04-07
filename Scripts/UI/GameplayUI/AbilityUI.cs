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
    [SerializeField] private Image cooldownBackground;

    private Coroutine cooldownCoroutine;

    private void Awake()
    {
        cooldownBackground.gameObject.SetActive(false);
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
        cooldownBackground.gameObject.SetActive(true);

        float timeRemaining = cooldownDuration;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            cooldownOverlay.fillAmount = timeRemaining / cooldownDuration;

            if (timeRemaining >= 1)
            {
                cooldownTimeNumericOverlay.text = timeRemaining.ToString("0");

            }else if (timeRemaining < 1)
            {
                cooldownTimeNumericOverlay.text = timeRemaining.ToString("0.0");

            }
            
            if(timeRemaining <= 0) cooldownTimeNumericOverlay.text = "";
            yield return null;
        }

        cooldownOverlay.fillAmount = 0; // Ensure it's fully reset
        cooldownBackground.gameObject.SetActive(false);
    }
    

    public AbilitySO GetAbilitySO() => ability;
}