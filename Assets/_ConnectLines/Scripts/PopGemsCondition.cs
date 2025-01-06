using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopRedGemsCondition", menuName = "WinConditions/PopRedGems")]
public class PopGemsCondition : WinCondition
{
    public int targetPopCount;
    private int currentCount = 0;
    
    private Dictionary<GemType, int> poppedGems = new Dictionary<GemType, int>
    {
        {GemType.Red, 0},
        {GemType.Blue, 0},
        {GemType.Green, 0},
    };
    
    public void IncrementCount(GemType gemType)
    { 
        poppedGems[gemType]++;
    }

    public override bool IsConditionMet()
    {
        return poppedGems[gemType] >= targetPopCount;
    }

    public override string GetProgress()
    {
        return $"{poppedGems[gemType]}/{targetPopCount} {gemType} gems popped";
    }
}