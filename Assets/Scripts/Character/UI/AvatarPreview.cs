using TotemEnums;
using UnityEngine;

public class AvatarPreview : MonoBehaviour
{
    [SerializeField] private Material skinMaterial;
    [SerializeField] private Material eyesMaterial;
    [SerializeField] private Material hairMaterial;
    [SerializeField] private Material clothesMaterial;
    [SerializeField] private HairStyleEnumGameObjectDictionary hairStyles;
    [SerializeField] private SexEnum characterGender;

    public HairStyleEnumGameObjectDictionary HairStyles { get => hairStyles; }
    public SexEnum CharacterGender { get => characterGender; }

    private HairStyle currentHairStyle = HairStyle.Short;

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

    public void ApplyClothesColor(Color clothesColor)
    {
        clothesMaterial.color = clothesColor;
    }

    public void ApplyHairStyle(HairStyle newHairStyle)
    {
        hairStyles[currentHairStyle].SetActive(false);
        hairStyles[newHairStyle].SetActive(true);
        currentHairStyle = newHairStyle;
    }
}
