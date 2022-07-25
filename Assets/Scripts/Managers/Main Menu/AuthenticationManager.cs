using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using TotemServices;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    private const string ACCESS_TOKEN_PREF_KEY = "socialLoginAccessToken";

    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject logInInProgressPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private TotemDB totemDB;
    private string gameId = "MonkVsRobots";

    private string accessToken;
    private string publicKey;

    private bool avatarsLoaded;
    private bool spearsLoaded;
    
    public GameObject LogInPanel
    {
        get
        {
            return logInPanel;
        }
    }

    public static AuthenticationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        totemDB = new TotemDB(gameId);

        totemDB.OnSocialLoginCompleted.AddListener(OnUserLoggedIn);
        totemDB.OnUserProfileLoaded.AddListener(OnUserProfileLoaded);
        totemDB.OnSpearsLoaded.AddListener(OnSpearsLoaded);
        totemDB.OnAvatarsLoaded.AddListener(OnAvatarsLoaded);

        if (!TotemManager.Instance.userAuthenticated && PlayerPrefs.HasKey(ACCESS_TOKEN_PREF_KEY))
        {
            var accessToken = PlayerPrefs.GetString(ACCESS_TOKEN_PREF_KEY);
            if (!string.IsNullOrEmpty(accessToken))
            {
                logInInProgressPanel.SetActive(true);
                totemDB.GetUserProfile(accessToken);
            }
        }
    }

    private void OnUserLoggedIn(TotemAccountGateway.SocialLoginResponse logInResult)
    {
        accessToken = logInResult.accessToken;
        PlayerPrefs.SetString(ACCESS_TOKEN_PREF_KEY, accessToken);
        totemDB.GetUserProfile(accessToken);
    }

    private void OnUserProfileLoaded(string userPublicKey)
    {
        publicKey = userPublicKey;

        TotemManager.Instance.userAuthenticated = true;
        avatarsLoaded = false;
        spearsLoaded = false;

        totemDB.GetUserAvatars(publicKey);
        totemDB.GetUserSpears(publicKey);
    }

    private void OnSpearsLoaded(List<TotemSpear> spears)
    {
        TotemManager.Instance.currentUserSpears = spears;
        spearsLoaded = true;

        if (avatarsLoaded)
        {
            logInInProgressPanel.SetActive(false);
            logInPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    private void OnAvatarsLoaded(List<TotemAvatar> avatars)
    {
        TotemManager.Instance.currentUserAvatars = avatars;
        avatarsLoaded = true;

        if (spearsLoaded)
        {
            logInInProgressPanel.SetActive(false);
            logInPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void OnLogInClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        logInInProgressPanel.SetActive(true);
        totemDB.AuthenticateCurrentUser();
    }

    public void LogOut()
    {
        TotemManager.Instance.userAuthenticated = false;
        accessToken = publicKey = string.Empty;
        TotemManager.Instance.currentUserAvatars.Clear();
        TotemManager.Instance.currentUserSpears.Clear();
        PlayerPrefs.SetString(ACCESS_TOKEN_PREF_KEY, accessToken);
        logInPanel.SetActive(true);
    }
}
