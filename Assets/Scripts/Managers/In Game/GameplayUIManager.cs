using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Opsive.UltimateCharacterController.Traits;
using Opsive.Shared.Events;

public class GameplayUIManager : MonoBehaviour
{
    #region Panels

    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject waveCountdownPanel;
    [SerializeField] private GameObject waveEnemiesPanel;

    #endregion

    #region InfoFields

    [SerializeField] private TMP_Text waveCountdownText;
    [SerializeField] private TMP_Text killedEnemiesText;
    [SerializeField] private TMP_Text totalWaveEnemiesText;
    [SerializeField] private TMP_Text currentWaveText;

    #endregion

    private int minutesUntilNextWave = 0;
    private int secondsUntilNextWave = 0;
    private int killedEnemiesCount = 0;

    public static GameplayUIManager Instance { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
        EventHandler.RegisterEvent<int>("StartNewWave", OnStartNewWave);
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        switch (gameEndedType)
        {
            case GameEndedType.Win:
                ShowHidePanel(winPanel, true);
                break;
            case GameEndedType.Lose:
                ShowHidePanel(losePanel, true);
                break;
        }
    }

    private void ShowHidePanel(GameObject panel, bool isActive)
    {
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
        panel.SetActive(isActive);
    }

    private void ConvertWaveTime(int waveTime)
    {
        minutesUntilNextWave = waveTime / 60;
        secondsUntilNextWave = waveTime % 60;
    }

    private string ConvertToTimeFormat(int value)
    {
        return value < 10 ? $"0{value}" : value.ToString();
    }

    private IEnumerator WaveCountdown()
    {
        yield return new WaitForSeconds(1);
        if(secondsUntilNextWave==0)
        {
            secondsUntilNextWave = 60;
            minutesUntilNextWave--;
        }
        secondsUntilNextWave--;
        waveCountdownText.text = $"{ConvertToTimeFormat(minutesUntilNextWave)} : {ConvertToTimeFormat(secondsUntilNextWave)}";

        if (secondsUntilNextWave != 0 || minutesUntilNextWave != 0)
        {
            StartCoroutine(WaveCountdown());
        }
    }

    private void OnStartNewWave(int currentWave)
    {
        killedEnemiesCount = 0;
        killedEnemiesText.text = "0";
        currentWaveText.text = $"Wave {currentWave+1}";
        waveCountdownPanel.SetActive(false);
        waveEnemiesPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent<GameEndedType>("GameEnded", OnGameEnded);
        EventHandler.UnregisterEvent<int>("StartNewWave", OnStartNewWave);
    }

    public void StartWaveCountdown(int timeUntilNextWave)
    {
        ConvertWaveTime(timeUntilNextWave);
        waveCountdownText.text = $"{ConvertToTimeFormat(minutesUntilNextWave)} : {ConvertToTimeFormat(secondsUntilNextWave)}";

        waveEnemiesPanel.SetActive(false);
        waveCountdownPanel.SetActive(true);
        StartCoroutine(WaveCountdown());
    }

    public void SetTotalEnemies(int totalEnemies)
    {
        totalWaveEnemiesText.text = totalEnemies.ToString();
    }

    public void OnRobotDied()
    {
        killedEnemiesCount++;
        killedEnemiesText.text = killedEnemiesCount.ToString();
    }

    public void OnRestartClicked()
    {
        ShowHidePanel(losePanel, false);
        EventHandler.ExecuteEvent("GameRestarted");
    }

    public void OnExitClick()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
