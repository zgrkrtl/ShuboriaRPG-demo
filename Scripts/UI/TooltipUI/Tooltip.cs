using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;


[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private LayoutElement layoutElement;

    private RectTransform rectTransform;

    private void Awake()
    {
        layoutElement = gameObject.GetComponent<LayoutElement>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    public void SetText(string content, string header, string headerColor, string contentColor)
    {
        contentField.gameObject.SetActive(true);
        headerField.gameObject.SetActive(true);
        
        headerField.text = $"<color={headerColor}>{header}</color>"; 
        contentField.text = $"<color={contentColor}>{content}</color>"; 
    
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }
   

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLenghth = headerField.text.Length;
            int contentLenghth = contentField.text.Length;
            
            layoutElement.enabled = headerLenghth > characterWrapLimit || contentLenghth > characterWrapLimit;
        }
        
        Vector2 position = Input.mousePosition;
        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
