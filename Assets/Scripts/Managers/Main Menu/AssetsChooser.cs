using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using UnityEngine;
using UnityEngine.UI;

public class AssetsChooser : MonoBehaviour
{
    [SerializeField] private Image avatarPreview;
    [SerializeField] private Image spearPreview;

    [SerializeField] private Button nextAvatarButton;
    [SerializeField] private Button previousAvatarButton;
    [SerializeField] private Button nextSpearButton;
    [SerializeField] private Button previousSpearButton;
    [SerializeField] private Button playButton;

    [SerializeField] private TMP_Text bodyTypeInfoTMP;
    [SerializeField] private TMP_Text spearDamageLvlTMP;
    [SerializeField] private TMP_Text spearRangeLvlTMP;

    [SerializeField] private TMP_Text currentSpearIndexTMP;
    [SerializeField] private TMP_Text maxSpearsCountTMP;
    [SerializeField] private TMP_Text currentAvatarIndexTMP;
    [SerializeField] private TMP_Text maxAvatarsCountTMP;

    [SerializeField] private AvatarPreviewManager avatarPreviewManager;
    [SerializeField] private WeaponPreviewManager weaponPreviewManager;

    #region SpearParamsVisualization

    [Min(1)]
    [SerializeField] private int spearDamageLvlStep;
    [Min(1)]
    [SerializeField] private int spearDamageMaxLvl;
    [Min(1)]
    [SerializeField] private int spearRangeLvlStep;
    [Min(1)]
    [SerializeField] private int spearRangeMaxLvl;

    #endregion

    private int spearPreviewIndex = 0;
    private int avatarPreviewIndex = 0;

    private List<TotemSpear> userSpears = new List<TotemSpear>();
    private List<TotemAvatar> userAvatars = new List<TotemAvatar>();

    public void OnEnable()
    {
        playButton.interactable = true;
        InitializeAssets();
        VerifyAssets();
    }

    public void OnDisable()
    {
        bodyTypeInfoTMP.text = string.Empty;
        spearDamageLvlTMP.text = string.Empty;
        spearRangeLvlTMP.text = string.Empty;
        avatarPreviewIndex = 0;
        spearPreviewIndex = 0;
    }

    public void OnAvatarPreviewChange(int indexStep)
    {
        avatarPreviewIndex += indexStep;
        avatarPreviewIndex = CheckPreviewIndex(avatarPreviewIndex, userAvatars.Count);
        currentAvatarIndexTMP.text = (avatarPreviewIndex+1).ToString();
        ChangeAvatar();
    }

    public void OnSpearPreviewChange(int indexStep)
    {
        spearPreviewIndex += indexStep;
        spearPreviewIndex = CheckPreviewIndex(spearPreviewIndex, userSpears.Count);
        currentSpearIndexTMP.text = (spearPreviewIndex + 1).ToString();
        ChangeSpear();
    }

    public void ClearUserData()
    {
        userSpears = new List<TotemSpear>();
        userAvatars = new List<TotemAvatar>();
    }

    public void SetChoosedAssets()
    {
        TotemManager.Instance.currentAvatar = userAvatars[avatarPreviewIndex];
        TotemManager.Instance.currentSpear = userSpears[spearPreviewIndex];
    }

    private int CheckPreviewIndex(int currentValue, int maxValue)
    {
        if (currentValue >= maxValue)
        {
            return 0;
        }
        else if (currentValue < 0)
        {
            return maxValue - 1;
        }

        return currentValue;
    }

    private void ChangeAvatar()
    {
        var newAvatarPreview = userAvatars[avatarPreviewIndex];
        var currentBodyType = avatarPreviewManager.ApplyAvatarPreviewChanges(newAvatarPreview);

        switch(currentBodyType)
        {
            case CharacterType.FatMuscular:
                bodyTypeInfoTMP.text = Constants.FATMUSCULAR_AVATAR_INFO;
                break;
            case CharacterType.FatWimp:
                bodyTypeInfoTMP.text = Constants.FATWIMP_AVATAR_INFO;
                break;
            case CharacterType.ThinMuscular:
                bodyTypeInfoTMP.text = Constants.THINMUSCULAR_AVATAR_INFO;
                break;
            case CharacterType.ThinWimp:
                bodyTypeInfoTMP.text = Constants.THINWIMP_AVATAR_INFO;
                break;
        }
    }

    private void ChangeSpear()
    {
        var newSpearPreview = userSpears[spearPreviewIndex];
        weaponPreviewManager.ApplyWeaponPreview(newSpearPreview);

        spearDamageLvlTMP.text = CalculateParameterLvl(newSpearPreview.damage, spearDamageLvlStep, spearDamageMaxLvl);
        spearRangeLvlTMP.text = CalculateParameterLvl(newSpearPreview.range, spearRangeLvlStep, spearRangeMaxLvl);
    }

    private void InitializeAssets()
    {
        userSpears = TotemManager.Instance.currentUser.GetOwnedSpears();
        userAvatars = TotemManager.Instance.currentUser.GetOwnedAvatars();
        maxSpearsCountTMP.text = userSpears.Count.ToString();
        maxAvatarsCountTMP.text = userAvatars.Count.ToString();
    }

    private bool CheckAvaliableAssets<T>(List<T> asstetsList, Button nextButton, Button previousButton)
    {
        if (asstetsList == null || asstetsList.Count == 0)
        {
            if(playButton.interactable)
            {
                playButton.interactable = false;
            }
            nextButton.interactable = false;
            previousButton.interactable = false;
            return false;
        }
        return true;
    }

    private void VerifyAssets()
    {
        if (CheckAvaliableAssets(userSpears, nextSpearButton, previousSpearButton))
        {
            ChangeSpear();
            currentSpearIndexTMP.text = "1";
        }
        else
        {
            currentSpearIndexTMP.text = "0";
        }

        if (CheckAvaliableAssets(userAvatars, nextAvatarButton, previousAvatarButton))
        {
            ChangeAvatar();
            currentAvatarIndexTMP.text = "1";
        }
        else
        {
            currentAvatarIndexTMP.text = "0";
        }
    }

    private string CalculateParameterLvl(float parameterValue, int lvlStep, int maxLvl)
    {
        var totalStars = (int)(parameterValue / lvlStep);

        if (totalStars > maxLvl)
        {
            totalStars = maxLvl;
        }
        if (totalStars == 0)
        {
            totalStars = 1;
        }

        string lvl = new string('+', totalStars);

        return lvl;
    }
}
