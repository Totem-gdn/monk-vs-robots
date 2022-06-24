using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotsSpawner : MonoBehaviour
{
    [Min(1f)]
    [SerializeField] private float spawnAreaScaleX = 1;
    [Min(1f)]
    [SerializeField] private float spawnAreaScaleZ = 1;
    [Min(0.1f)]
    [SerializeField] private float spawnInterval = 0.1f;
    [SerializeField] private Transform spawnPointCenter;
    [SerializeField] private List<GameObject> wavesPool = new List<GameObject>();

    private List<List<BaseStateMachine>> wavesRobots = new List<List<BaseStateMachine>>();
    private List<BaseStateMachine> currentWaveRobots;
    private int currentWave = 0;
    private int spawnedEnemies = 0;
    private float spawnAreaHalfScaleX = 0;
    private float spawnAreaHalfScaleZ = 0;

    void Awake()
    {
        spawnAreaHalfScaleX = spawnAreaScaleX / 2;
        spawnAreaHalfScaleZ = spawnAreaScaleZ / 2;
        InitializeWaves();

        EventHandler.RegisterEvent<int>("StartNewWave", OnStartNewWave);
        EventHandler.RegisterEvent<BaseStateMachine>(this, "RobotDied", OnRobotDied);
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);
    }

    private void InitializeWaves()
    {
        for (int i = 0; i < wavesPool.Count; i++)
        {
            List<BaseStateMachine> waveRobots = new List<BaseStateMachine>();
            if (wavesPool[i] != null)
            {
                waveRobots = wavesPool[i].GetComponentsInChildren<BaseStateMachine>(true).ToList();
                foreach(var robot in waveRobots)
                {
                    robot.spawnerRoot = this;
                }
            }
            wavesRobots.Add(waveRobots);
        }
    }

    private void OnStartNewWave(int waveIndex)
    {
        if (waveIndex < wavesPool.Count)
        {
            currentWave = waveIndex;
            EventHandler.ExecuteEvent(WavesManager.Instance,"GetWaveEnemiesCount", wavesRobots[waveIndex].Count);
            if (wavesRobots[waveIndex].Count > 0)
            {
                //Activate portal animation
                currentWaveRobots = wavesRobots[waveIndex];
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        var enemyToSpawn = currentWaveRobots[spawnedEnemies];
        enemyToSpawn.transform.SetParent(null);
        enemyToSpawn.transform.position = CalculateSpawnPosition(enemyToSpawn.transform.lossyScale.y);
        enemyToSpawn.transform.eulerAngles = transform.rotation.eulerAngles;
        enemyToSpawn.gameObject.SetActive(true);
        enemyToSpawn.ActivateStartState();
        spawnedEnemies++;

        yield return new WaitForSeconds(spawnInterval);

        if(spawnedEnemies < currentWaveRobots.Count)
        {
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            //Disable portal animation
            spawnedEnemies = 0;
        }
    }

    private Vector3 CalculateSpawnPosition(float robotScaleY)
    {
        var randomizedValueX = Random.Range(-spawnAreaHalfScaleX, spawnAreaHalfScaleX);
        var randomizedValueZ = Random.Range(-spawnAreaHalfScaleZ, spawnAreaHalfScaleZ);

        return new Vector3(spawnPointCenter.position.x + randomizedValueX, 
            spawnPointCenter.position.y + robotScaleY/2, 
            spawnPointCenter.position.z + randomizedValueZ);
    }

    private void OnRobotDied(BaseStateMachine robotStateMachine)
    {
        robotStateMachine.transform.SetParent(wavesPool[currentWave].transform);
        robotStateMachine.transform.position = Vector3.zero;
        robotStateMachine.hpController.Heal(robotStateMachine.hpController.MaxHp);
        robotStateMachine.gameObject.SetActive(false);
    }

    private void OnGameRestarted()
    {
        foreach (var robot in currentWaveRobots)
        {
            robot.ResetStateMachine();
            robot.gameObject.SetActive(false);
            OnRobotDied(robot);
        }
        spawnedEnemies = 0;
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        if(gameEndedType == GameEndedType.Lose)
        {
            StopAllCoroutines();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventHandler.UnregisterEvent<int>("StartNewWave", OnStartNewWave);
        EventHandler.UnregisterEvent<BaseStateMachine>(gameObject, "RobotDied", OnRobotDied);
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
        EventHandler.UnregisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }
}
