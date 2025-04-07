using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D interactCursor;
    public Texture2D attackCursor;

    private string lastHitTag = ""; // Track the last hit tag to prevent repeated changes

    private void Start()
    {
        SetCursor(defaultCursor);
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            string hitTag = hit.collider.tag;
            

            if (hitTag != lastHitTag)
            {
                if (hitTag == "Enemy")
                {
                    SetCursor(attackCursor);
                }
                else if (hitTag == "Player")
                {
                    SetCursor(interactCursor);
                }
                else if (hitTag == "Untagged")
                {
                    SetCursor(defaultCursor);
                }
                else
                {
                    SetCursor(defaultCursor);
                }

                // Update the last hit tag to prevent unnecessary cursor changes
                lastHitTag = hitTag;
            }
        }
        else
        {
            SetCursor(defaultCursor);
            lastHitTag = ""; // Reset if no hit
        }
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorForUIElement()
    {
        SetCursor(interactCursor);
    }

    public void SetCursorDefault()
    {
        SetCursor(defaultCursor);
    }
}