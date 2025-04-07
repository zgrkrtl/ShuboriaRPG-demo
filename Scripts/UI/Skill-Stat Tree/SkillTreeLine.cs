using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeLine : MonoBehaviour
{
    
    
    [SerializeField] private RectTransform skillTreeComponent1;
    [SerializeField] private RectTransform skillTreeComponent2;
    [SerializeField] private RectTransform lineRectTransform;
    [SerializeField] private StatTreeManager statTreeManager;

    private Image buttonImage;

    private void Awake()
    {
        statTreeManager.OnReset += () => SetAlpha(0.15f);
        buttonImage = GetComponent<Image>();
        if(skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>())
        {
            skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>().OnUnlockedAction += () => { SetAlpha(1); };
            if (skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>().isUnlocked)
            {
                SetAlpha(1);
            }
        }
        else
        {
            skillTreeComponent2.GetComponent<StatsTreeComponent>().OnUnlockedAction += () => { SetAlpha(1); };
            if (skillTreeComponent2.GetComponent<StatsTreeComponent>().isUnlocked)
            {
                SetAlpha(1);
            }
        }
        
        if (skillTreeComponent1 == null || skillTreeComponent2 == null || lineRectTransform == null)
            return;
        
        // Calculate Midpoint
        Vector2 position1 = skillTreeComponent1.position;
        Vector2 position2 = skillTreeComponent2.position;
        lineRectTransform.position = (position1 + position2) / 2f;

        // Calculate Distance and Set Width
        float distance = Vector2.Distance(position1, position2);
        lineRectTransform.sizeDelta = new Vector2(distance, lineRectTransform.sizeDelta.y);

        // Calculate Rotation Angle
        Vector2 direction = position2 - position1;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Start()
    {
        if(skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>())
        {
            skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>().OnUnlockedAction += () => { SetAlpha(1); };
            if (skillTreeComponent2.GetComponent<UniqueStatsTreeComponent>().isUnlocked)
            {
                SetAlpha(1);
            }
        }
        else
        {
            skillTreeComponent2.GetComponent<StatsTreeComponent>().OnUnlockedAction += () => { SetAlpha(1); };
            if (skillTreeComponent2.GetComponent<StatsTreeComponent>().isUnlocked)
            {
                SetAlpha(1);
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        Color newColor = buttonImage.color;
        newColor.a = alpha;
        buttonImage.color = newColor;
    }
    
}