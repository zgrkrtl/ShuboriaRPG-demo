using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    [SerializeField] private string header;

    [Multiline()]
    [SerializeField] private string content;

    [SerializeField] private string headerColor = "green";
    [SerializeField] private string contentColor = "black";
    [SerializeField] private bool isItemSlot = false;
    
    private InventorySlot inventorySlot;
    
    private Coroutine showTooltipCoroutine;
        
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventorySlot = GetComponent<InventorySlot>();
        showTooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
            showTooltipCoroutine = null;
        }
        TooltipSystem.Hide();
    }

    private IEnumerator ShowTooltipWithDelay()
    {
        yield return new WaitForSeconds(0.25f);
        if (isItemSlot && inventorySlot.SlotItem != null)
        {
            header = inventorySlot.SlotItem.ColouredName;
            content = inventorySlot.InfoDisplay;
        }
        TooltipSystem.Show(content, header,headerColor,contentColor);
    }
}