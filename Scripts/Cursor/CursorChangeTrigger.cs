using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChangeTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CursorManager cursorManager;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorManager.SetCursorForUIElement();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       cursorManager.SetCursorDefault();
    }
}
