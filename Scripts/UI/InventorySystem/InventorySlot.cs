using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : ItemSlotUI,IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;

    public override HotbarItem SlotItem
    {
        get { return ItemSlot.item; }
        set {}
    }
    public ItemSlot ItemSlot => inventory.ItemContainer.GetSlotByIndex(SlotIndex);
    public string InfoDisplay => ItemSlot.item.GetItemInfo();

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler handler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if (handler == null) return;

        if ((handler.ItemSlotUI as InventorySlot) != null)
        {
            inventory.ItemContainer.Swap(handler.ItemSlotUI.SlotIndex, SlotIndex);
        }
    }

    public override void UpdateSlotUI()
    {
        if (ItemSlot.item == null)
        {
            EnableSlotUI(false);
            return;
        }
        EnableSlotUI(true);

        itemIconImage.sprite = ItemSlot.item.Icon;
        itemQuantityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }
}
