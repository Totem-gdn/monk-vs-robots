using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TotemEntities;
using TotemEntities.DNA;
using TotemEnums;
using TotemServices;
using TotemServices.DNA;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    private const string ACCESS_TOKEN_PREF_KEY = "socialLoginAccessToken";

    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject logInInProgressPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private AssetsChooser assetsChooser;

    private string gameId = "0xBdddAA60C2F104cC9f4c65dE5A11e9c08636daBC";
    private string _avatarsFilterJson;
    private string _itemsFilterJson;

    private bool _isAvatarsLoaded = false;
    private bool _isItemsLoaded = false;
    
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
            _avatarsFilterJson = Resources.Load<TextAsset>("totem-common-files/filters/monk-vs-robots-avatar").text;
            _itemsFilterJson = Resources.Load<TextAsset>("totem-common-files/filters/totem-item").text;
        }
    }

    private TotemAvatar LoadAvatar(TotemDNADefaultAvatar defaultAvatar)
    {
        TotemAvatar newAvatar = new TotemAvatar();

        newAvatar.bodyFat = defaultAvatar.body_type ? BodyFatEnum.Fat : BodyFatEnum.Thin;
        newAvatar.bodyMuscles = defaultAvatar.body_strength ? BodyMusclesEnum.Muscular : BodyMusclesEnum.Wimp;
        newAvatar.eyeColor = GetColorFromHex(defaultAvatar.human_eye_color);
        newAvatar.hairColor = GetColorFromHex(defaultAvatar.human_hair_color);
        newAvatar.skinColor = GetColorFromHex(defaultAvatar.human_skin_color);
        newAvatar.clothesColor = defaultAvatar.primary_color;
        newAvatar.sex = defaultAvatar.sex_bio ? SexEnum.Male : SexEnum.Female;
        defaultAvatar.hair_styles = defaultAvatar.hair_styles.Replace(" ",string.Empty);
        Enum.TryParse(defaultAvatar.hair_styles, true, out newAvatar.hairStyle);

        return newAvatar;
    }

    private TotemSpear LoadSpear(TotemDNADefaultItem defaultSpear)
    {
        TotemSpear newSpear = new TotemSpear();

        newSpear.damage = GetFixedNumericValue(defaultSpear.damage_nd, assetsChooser.SpearDamageLvlStep);
        newSpear.range = GetFixedNumericValue(defaultSpear.range_nd, assetsChooser.SpearRangeLvlStep);
        newSpear.shaftColor = defaultSpear.primary_color;
        Enum.TryParse(defaultSpear.weapon_material, true, out newSpear.tipMaterial);
        Enum.TryParse(defaultSpear.classical_element, true, out newSpear.element);

        return newSpear;
    }

    private float GetFixedNumericValue(uint basicValue, float minValue)
    {
        float fixedValue = minValue;
        float newValue = (float)basicValue / (float)uint.MaxValue;
        newValue *= 100;

        if(newValue>minValue)
        {
            fixedValue = newValue;
        }

        return fixedValue;
    }

    private Color GetColorFromHex(string colorHex)
    {
        var resultColor = Color.black;
        if (!ColorUtility.TryParseHtmlString(colorHex, out resultColor))
        {
            resultColor = Color.black;
        }

        return resultColor;
    }

    private string GetFilter(string filterPath)
    {
        using(StreamReader sr = new StreamReader(filterPath))
        {
            return sr.ReadToEnd();
        }
    }

    private void OnLoginComplete()
    {
        if (_isAvatarsLoaded && _isItemsLoaded)
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

        TotemCore totemCore = new TotemCore(gameId);
        TotemDNAFilter avatarsFilter = new TotemDNAFilter(_avatarsFilterJson);
        TotemDNAFilter itemsFilter = new TotemDNAFilter(_itemsFilterJson);

        totemCore.AuthenticateCurrentUser((user) =>
        {
            totemCore.GetUserAvatars<TotemDNADefaultAvatar>(user, avatarsFilter, (avatars) =>
            {
                foreach (var avatar in avatars)
                {
                    TotemManager.Instance.currentUserAvatars.Add(LoadAvatar(avatar));
                }
                _isAvatarsLoaded = true;
                OnLoginComplete();
            });

            totemCore.GetUserItems<TotemDNADefaultItem>(user, itemsFilter, (spears) =>
            {
                foreach(var spear in spears)
                {
                    TotemManager.Instance.currentUserSpears.Add(LoadSpear(spear));
                }
                _isItemsLoaded = true;
                OnLoginComplete();
            });
        });
    }

    public void LogOut()
    {
        TotemManager.Instance.userAuthenticated = false;
        TotemManager.Instance.currentUserAvatars.Clear();
        TotemManager.Instance.currentUserSpears.Clear();

        _isItemsLoaded = false;
        _isAvatarsLoaded = false;

        logInPanel.SetActive(true);
    }
}
