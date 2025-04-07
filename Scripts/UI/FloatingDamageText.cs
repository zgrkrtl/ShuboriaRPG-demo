using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float scaleAmplitude = 0.1f; // Smooth scale oscillation
    [SerializeField] private float scaleSpeed = 2f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float fixedScale = 0.2f; // Constant base scale

    private TextMeshPro textMeshPro;
    private Vector3 baseScale;
    private Color textColor;
    private float alphaValue = 1f;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();

        // Normalize scale and store base scale
        transform.localScale = Vector3.one * fixedScale;
        baseScale = transform.localScale;

        textColor = textMeshPro.color;

        StartCoroutine(AnimateText());
        Destroy(gameObject, destroyTime);
    }

    private IEnumerator AnimateText()
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedTime < destroyTime)
        {
            // Optional: Keep the text facing the camera
            transform.forward = Camera.main.transform.forward;

            // Maintain fixed rotation if needed
            transform.rotation = initialRotation;

            // Move upwards smoothly
            transform.position += Vector3.up * (moveSpeed * Time.deltaTime);

            // Smooth scale animation using sine wave
            float scaleFactor = 1f + Mathf.Sin(elapsedTime * scaleSpeed) * scaleAmplitude;
            transform.localScale = baseScale * scaleFactor;

            // Fade out the text over time
            alphaValue -= fadeSpeed * Time.deltaTime;
            textColor.a = Mathf.Clamp01(alphaValue);
            textMeshPro.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent at the end
        textColor.a = 0f;
        textMeshPro.color = textColor;
    }
    
}
