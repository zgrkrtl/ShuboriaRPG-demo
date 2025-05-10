using UnityEngine;

public abstract class InventoryItem : HotbarItem
{
    [Header("Item Data")]
    [SerializeField] [Min(0)] private int sellPrice = 1;
    [SerializeField] [Min(0)] private int maxStack = 1;
    
    [Multiline()]
    [SerializeField] private string itemInfo;
    public override string ColouredName
    {
        get
        {
            return Name;
        }
    }

    public string GetItemInfo()
    {
        return itemInfo;
    }
    public int SellPrice => sellPrice;
    public int MaxStack => maxStack;
}
