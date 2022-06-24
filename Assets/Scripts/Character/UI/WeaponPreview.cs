using enums;
using UnityEngine;
using DG.Tweening;

public class WeaponPreview : MonoBehaviour
{
    [SerializeField] private TipMaterialEnumGameObjectDictionary tipTypes;
    [SerializeField] private ElementEnumGameObjectDictionary elementTypes;
    [SerializeField] private Material spearShaftMaterial;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Vector3 rotateTo;
    [SerializeField] private float rotateAnimationTime;
    [SerializeField] private float moveAnimationTime;
    [SerializeField] private float moveY;

    private TipMaterialEnum currentTipMaterial;
    private ElementEnum element;

    private void Start()
    {
        //rootTransform.DORotate(rotateTo, rotateAnimationTime).SetLoops(-1, LoopType.Yoyo);
        //rootTransform.DOLocalMoveY(moveY, moveAnimationTime).SetLoops(-1, LoopType.Yoyo);
    }

    public void ApplyTipMaterial(TipMaterialEnum tipMaterialToSet)
    {
        tipTypes[currentTipMaterial].SetActive(false);
        currentTipMaterial = tipMaterialToSet;

        var newTip = tipTypes[currentTipMaterial];
        newTip.SetActive(true);
    }

    public void ApplyShaftColor(Color shaftColor)
    {
        if(spearShaftMaterial != null)
        {
            spearShaftMaterial.color = shaftColor;
        }
    }

    public void ApplySpearElement(ElementEnum elementToSet)
    {
        elementTypes[element].SetActive(false);
        element = elementToSet;

        elementTypes[elementToSet].SetActive(true);
    }
}
