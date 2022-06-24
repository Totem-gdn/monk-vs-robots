using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    [SerializeField] private int waveStartDelay = 0;
    [SerializeField] private int wavesCount = 0;

    private int totalWaveEnemies = 0;
    private int currentWave = 0;

    public static WavesManager Instance { get; private set; }

    void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }

        EventHandler.RegisterEvent<int>(this, "GetWaveEnemiesCount", OnGetWaveEnemiesCount);
        EventHandler.RegisterEvent(this, "RobotDied", OnRobotKilled);
        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);

        StartCoroutine(StartNewWave());
    }

    private void OnGetWaveEnemiesCount(int enemiesCount)
    {
        totalWaveEnemies += enemiesCount;
        GameplayUIManager.Instance.SetTotalEnemies(totalWaveEnemies);
    }

    private void OnRobotKilled()
    {
        totalWaveEnemies--;
        GameplayUIManager.Instance.OnRobotDied();
        if (totalWaveEnemies == 0)
        {
            if (currentWave+1 < wavesCount)
            {
                currentWave++;
                StartCoroutine(StartNewWave());
            }
            else
            {
                EventHandler.ExecuteEvent("GameEnded", GameEndedType.Win);
            }
        }
    }

    private IEnumerator StartNewWave()
    {
        GameplayUIManager.Instance.StartWaveCountdown(waveStartDelay);
        yield return new WaitForSeconds(waveStartDelay);
        EventHandler.ExecuteEvent("StartNewWave", currentWave);
    }

    private void OnGameRestarted()
    {
        currentWave = 0;
        totalWaveEnemies = 0;
        StartCoroutine(StartNewWave());
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent<int>(this, "GetWaveEnemiesCount", OnGetWaveEnemiesCount);
        EventHandler.UnregisterEvent(this, "RobotKilled", OnRobotKilled);
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
    }
}
