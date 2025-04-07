using System;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
  
    private static TooltipSystem current;
    [SerializeField] private Tooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header,string headerColor, string contentColor)
    {
       
        current.tooltip.SetText(content, header,headerColor,contentColor);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
    
}
