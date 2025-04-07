using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LevelAndExperienceUI : MonoBehaviour
{
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private Image fillableOne;
    [SerializeField] private Image fillableTwo;
    [SerializeField] private Image frontImage;
    [SerializeField] private TextMeshProUGUI levelText;
    
    private Coroutine fillAnimationCoroutine;
    private MandatoryData mandatoryData;
    private void Start()
    {
        mandatoryData = MandatoryDataSaveManager.Load();
        gameplayManager.OnExperienceAndLevelChange += OnExperienceAndLevelChange;
        
        UpdateExperienceUI(mandatoryData.experiencePoints / gameplayManager.ExperienceRequiredForLevel(mandatoryData.level + 1));
        levelText.text = mandatoryData.level.ToString();
    }

    private void OnExperienceAndLevelChange(int level, float expPercent)
    {
        UpdateExperienceUI(expPercent);
        levelText.text = level.ToString();
    }
    
    private void UpdateExperienceUI(float percentage)
    {
        Color targetColor = GetColorFromPercentage(percentage);

        // Stop any previous running coroutine
        if (fillAnimationCoroutine != null)
            StopCoroutine(fillAnimationCoroutine);

        fillAnimationCoroutine = StartCoroutine(AnimateFillAndColor(percentage, targetColor));
    }
    
    private IEnumerator AnimateFillAndColor(float targetPercentage, Color targetColor)
    {
        float duration = 0.5f; // Duration of the animation
        float timeElapsed = 0f;

        float startPercentage = fillableOne.fillAmount;
        Color startColor = fillableOne.color;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            float currentPercentage = Mathf.Lerp(startPercentage, targetPercentage, t);
            Color currentColor = Color.Lerp(startColor, targetColor, t);

            fillableOne.fillAmount = currentPercentage;
            fillableTwo.fillAmount = currentPercentage;

            fillableOne.color = currentColor;
            fillableTwo.color = currentColor;
            frontImage.color = currentColor;

            yield return null;
        }

        // Ensure final state is set
        fillableOne.fillAmount = targetPercentage;
        fillableTwo.fillAmount = targetPercentage;

        fillableOne.color = targetColor;
        fillableTwo.color = targetColor;
        frontImage.color = targetColor;
    }
    private Color GetColorFromPercentage(float percentage)
    {
        Color startColor = HexToColor("#FF0000"); // Red
        Color endColor = HexToColor("#00FF00");   // Green
        return Color.Lerp(startColor, endColor, percentage);
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        return Color.white; // Default if hex is invalid
    }
}
