using System;
using System.Collections;
using System.Collections.Generic;
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

    private string gameId = "MonkVsRobots";
    
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
    }

    private TotemAvatar LoadAvatar(TotemDNADefaultAvatar defaultAvatar)
    {
        TotemAvatar newAvatar = new TotemAvatar();

        newAvatar.bodyFat = defaultAvatar.body_type ? BodyFatEnum.Fat : BodyFatEnum.Thin;
        newAvatar.bodyMuscles = defaultAvatar.body_strength ? BodyMusclesEnum.Muscular : BodyMusclesEnum.Wimp;
        newAvatar.eyeColor = GetColorFromHex(defaultAvatar.human_eye_color);
        newAvatar.hairColor = GetColorFromHex(defaultAvatar.human_hair_color);
        newAvatar.skinColor = GetColorFromHex(defaultAvatar.human_skin_color);
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

    public void OnLogInClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        logInInProgressPanel.SetActive(true);

        TotemCore totemCore = new TotemCore(gameId);

        totemCore.AuthenticateCurrentUser(Provider.GOOGLE, (user) =>
        {
            totemCore.GetUserAvatars<TotemDNADefaultAvatar>(user, TotemDNAFilter.DefaultAvatarFilter, (avatars) =>
            {
                foreach (var avatar in avatars)
                {
                    TotemManager.Instance.currentUserAvatars.Add(LoadAvatar(avatar));
                }
            });

            totemCore.GetUserItems<TotemDNADefaultItem>(user, TotemDNAFilter.DefaultItemFilter, (spears) =>
            {
                foreach(var spear in spears)
                {
                    TotemManager.Instance.currentUserSpears.Add(LoadSpear(spear));
                }
            });

            logInInProgressPanel.SetActive(false);
            logInPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        });
    }

    public void LogOut()
    {
        TotemManager.Instance.userAuthenticated = false;
        TotemManager.Instance.currentUserAvatars.Clear();
        TotemManager.Instance.currentUserSpears.Clear();
        logInPanel.SetActive(true);
    }
}
