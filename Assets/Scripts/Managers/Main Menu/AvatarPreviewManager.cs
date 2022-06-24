using enums;
using System;
using TotemEntities;
using UnityEngine;
using System.Linq;

public class AvatarPreviewManager : MonoBehaviour
{
    [SerializeField] private CharacterTypeGameObjectDictionary charactersTypesMale;
    [SerializeField] private CharacterTypeGameObjectDictionary charactersTypesFemale;

    private SexEnum? currentSex = null;
    private CharacterType currentCharacterType;
    private CharacterTypeGameObjectDictionary charactersTypes;
    [SerializeField] private AvatarPreview currentAvatarPreview;

    public CharacterType ApplyAvatarPreviewChanges(TotemAvatar choosedAvatar)
    {
        ApplyGender(choosedAvatar.sex);
        ApplyCharacterBodyType(choosedAvatar.bodyFat, choosedAvatar.bodyMuscles);
        currentAvatarPreview = charactersTypes[currentCharacterType].GetComponent<AvatarPreview>();
        currentAvatarPreview.ApplyEyesColor(choosedAvatar.eyeColor);
        currentAvatarPreview.ApplyHairColor(choosedAvatar.hairColor);
        currentAvatarPreview.ApplySkinColor(choosedAvatar.skinColor);
        currentAvatarPreview.ApplyHairStyle(choosedAvatar.hairStyle);

        return currentCharacterType;
    }

    private void ApplyGender(SexEnum sex)
    {
        if (sex != currentSex)
        {
            if (charactersTypes != null)
            {
                foreach (var item in charactersTypes)
                {
                    item.Value.SetActive(false);
                }
            }
            switch (sex)
            {
                case SexEnum.Male:
                    charactersTypes = charactersTypesMale;
                    break;
                case SexEnum.Female:
                    charactersTypes = charactersTypesFemale;
                    break;
            }
        }
        currentSex = sex;
    }

    private void ApplyCharacterBodyType(BodyFatEnum bodyFat, BodyMusclesEnum bodyMuscles)
    {
        CharacterType newCharacterType = (CharacterType)Enum.Parse(typeof(CharacterType),
            bodyFat.ToString() + bodyMuscles.ToString());
        charactersTypes[currentCharacterType].SetActive(false);
        charactersTypes[newCharacterType].SetActive(true);
        currentCharacterType = newCharacterType;
    }
}
