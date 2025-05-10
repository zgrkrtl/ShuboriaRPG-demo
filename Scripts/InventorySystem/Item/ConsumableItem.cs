using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory System/ConsumableItem")]
public class ConsumableItem : InventoryItem
{
    [Header("Consumable Data")] [SerializeField]
    private string useText = "Consume This!";
    
    public override string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Name).AppendLine();
        builder.Append("<color=green>Use: </color>").Append(useText).Append("</color>").AppendLine();
        builder.Append("Max Stack: ").AppendLine(MaxStack.ToString()).AppendLine();
        builder.Append("Sell Price: ").AppendLine(SellPrice.ToString()).Append("Gold");
        
        return builder.ToString();
    }
}
