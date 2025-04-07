using System;

[System.Serializable]
public class SkillTreeData
{
    public bool[] UnlockedStats;

    public SkillTreeData()
    {
        // Initialize UnlockedStats array with 46 elements, all set to false
        UnlockedStats = new bool[46];
    }
}