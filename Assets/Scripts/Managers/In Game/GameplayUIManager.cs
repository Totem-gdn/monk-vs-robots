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
    [SerializeField] private GameObject exitPanel;

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
    private bool isGameEnded = false;
    private List<GameObject> activePanels = new List<GameObject>();

    public static GameplayUIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
        EventHandler.RegisterEvent<int>("StartNewWave", OnStartNewWave);
    }

    private void Update()
    {
        if(!isGameEnded && Input.GetKeyUp(KeyCode.Escape))
        {
            if (!exitPanel.activeSelf)
            {
                ShowPanel(exitPanel);
            }
            else
            {
                HidePanel(exitPanel);
            }
        }
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        CloseActivePanels();
        isGameEnded = true;
        switch (gameEndedType)
        {
            case GameEndedType.Win:
                ShowPanel(winPanel);
                break;
            case GameEndedType.Lose:
                ShowPanel(losePanel);
                break;
        }
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

    private void CloseActivePanels()
    {
        foreach(var panel in activePanels)
        {
            panel.SetActive(false);
        }
        activePanels.Clear();
    }

    private void EnableDisableCursor(bool isActive)
    {
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }

    public void ShowPanel(GameObject panel)
    {
        EnableDisableCursor(true);
        activePanels.Add(panel);
        panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        EnableDisableCursor(false);
        activePanels.Remove(panel);
        panel.SetActive(false);
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
        AudioManager.Instance?.PlayButtonSound();
        HidePanel(losePanel);
        isGameEnded = false;
        EventHandler.ExecuteEvent("GameRestarted");
    }

    public void OnExitClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        CloseActivePanels();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
