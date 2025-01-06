using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int movesUsed = 0;
    private int moveLimit;
    private WinCondition[] winConditions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartLevel(LevelData levelData)
    {
        moveLimit = levelData.moveLimit;
        winConditions = levelData.conditions;

        movesUsed = 0;
        
        UpdateUI();
    }

    public void UseMove()
    {
        movesUsed++;
        CheckWinConditions();
    }

    public void NotifyConditionProgress(WinCondition condition)
    {
        CheckWinConditions();
        UpdateUI();
    }

    private void CheckWinConditions()
    {
        if (movesUsed >= moveLimit)
        {
            Debug.Log("You lost");
            return;
        }

        foreach (var condition in winConditions)
        {
            if (!condition.IsConditionMet())
            {
                return;
            }
            Debug.Log("You Win");
        }
    }
    
    private void UpdateUI()
    {
        // Update moves and conditions in the UI
        Debug.Log($"Moves Left: {moveLimit - movesUsed}");
        foreach (WinCondition condition in winConditions)
        {
            Debug.Log(condition.GetProgress());
        }
    }

    public WinCondition[] GetCurrentConditions()
    {
        return winConditions;
    }
}
