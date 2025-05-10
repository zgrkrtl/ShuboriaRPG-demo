using UnityEngine;

public interface IItemContainer
{
    public ItemSlot AddItem(ItemSlot itemSlot);
    public void RemoveItem(ItemSlot itemSlot);
    public void RemoveAt(int slotIndex);
    public void Swap(int indexOne, int indexTwo);
    public bool HasItem(InventoryItem item);
    public int GetTotalQuantity(InventoryItem item);
}
