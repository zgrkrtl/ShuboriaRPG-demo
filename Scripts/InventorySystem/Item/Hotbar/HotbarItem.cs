using UnityEngine;

public abstract class HotbarItem : ScriptableObject
{
    [Header("Basic Information")] 
    [SerializeField] private new string name = "New Hotbar Item Name";
    [SerializeField] private Sprite icon = null;
    
    public string Name => name;
    public abstract string ColouredName { get; }
    public Sprite Icon => icon;

    public abstract string GetInfoDisplayText();
}
