using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelsTimeConfigurations 
    {
        public float timeForNextLevel;
    }

    public System.Action OnNewLevelStarted;

    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] ShipPlayer player;
    [Header("Configuration")]
    [SerializeField] private List<LevelsTimeConfigurations> timesConfigurations;

    private int currentLevel = -1;
    private float currentLevelTime = 0;
    private IEnumerator timeIEnumerator;

    public void StartNewLevel() 
    {
        IncreaseCurrentLevel();
        SetCurrentLevel();
        StartCurrentLevel();
    }

    private void IncreaseCurrentLevel() 
    {
        currentLevel++;
        if (currentLevel == timesConfigurations.Count)
        {
            currentLevel = 0;
            currentLevelTime = 0;
        }
    }

    private void SetCurrentLevel() 
    {
        enemyManager.SetNewLevel(currentLevel, player);
    }

    private void StartCurrentLevel()
    {
        enemyManager.StartCurrentLevel();

        if (timeIEnumerator != null) StopCoroutine(timeIEnumerator);
        timeIEnumerator = TimeCoroutine();
        StartCoroutine(timeIEnumerator);

        OnNewLevelStarted?.Invoke();
    }

    private IEnumerator TimeCoroutine() 
    {
        while (currentLevelTime < timesConfigurations[currentLevel].timeForNextLevel) 
        {
            currentLevelTime += Time.deltaTime;
            yield return null;
        }
        StartNewLevel();
    }
}