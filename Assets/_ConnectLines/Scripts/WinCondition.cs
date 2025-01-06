using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WinCondition : ScriptableObject
{
    public GemType gemType;
    public abstract bool IsConditionMet();
    public abstract string GetProgress();
}