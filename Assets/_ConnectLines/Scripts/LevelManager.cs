using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData[] levels; // All levels in the game
    private int currentLevelIndex = 0;           // Current level index

    public LevelData CurrentLevel => levels[currentLevelIndex]; // Access current level data

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        LoadLevel(0);
        
        foreach (var condition in levels[0].conditions)
        {
            Debug.Log($"Win Condition: {condition.GetProgress()}");
        }
        
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        currentLevelIndex = levelIndex;
        Debug.Log($"Loading Level {levelIndex + 1}");

        // Pass the current level data to GameManager
        GameManager.Instance.StartLevel(CurrentLevel);
    }

    public bool HasNextLevel()
    {
        return currentLevelIndex < levels.Length - 1;
    }

    public void LoadNextLevel()
    {
        if (HasNextLevel())
        {
            LoadLevel(currentLevelIndex + 1);
        }
        else
        {
            Debug.Log("No more levels! Game Over.");
            // Handle end of game logic
        }
    }
}