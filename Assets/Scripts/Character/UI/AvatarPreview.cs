using enums;
using UnityEngine;

public class AvatarPreview : MonoBehaviour
{
    [SerializeField] private Material skinMaterial;
    [SerializeField] private Material eyesMaterial;
    [SerializeField] private Material hairMaterial;
    [SerializeField] private HairStyleEnumGameObjectDictionary hairStyles;
    [SerializeField] private SexEnum characterGender;

    public HairStyleEnumGameObjectDictionary HairStyles { get => hairStyles; }
    public SexEnum CharacterGender { get => characterGender; }

    private HairStyleEnum currentHairStyle = HairStyleEnum.Short;

    public void ApplySkinColor(Color skinColor)
    {
        skinMaterial.color = skinColor;
    }

    public void ApplyEyesColor(Color eyesColor)
    {
        eyesMaterial.color = eyesColor;
    }

    public void ApplyHairColor(Color hairColor)
    {
        hairMaterial.color = hairColor;
    }

    public void ApplyHairStyle(HairStyleEnum newHairStyle)
    {
        hairStyles[currentHairStyle].SetActive(false);
        hairStyles[newHairStyle].SetActive(true);
        currentHairStyle = newHairStyle;
    }
}
