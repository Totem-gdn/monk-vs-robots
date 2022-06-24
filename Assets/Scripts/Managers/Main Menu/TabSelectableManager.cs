using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSelectableManager : MonoBehaviour
{
    [SerializeField] private List<Selectable> selectableToTab;

    private int currentSelectableIndex = 0;

    private void Start()
    {
        selectableToTab[currentSelectableIndex].Select();
    }

    private void Update()
    {
        DetectTabClick();
    }

    public void DetectTabClick()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentSelectableIndex + 1 >= selectableToTab.Count)
            {
                currentSelectableIndex = 0;
            }
            else
            {
                currentSelectableIndex++;
            }

            if (selectableToTab.Count > 0)
            {
                selectableToTab[currentSelectableIndex].Select();
            }
        }
    }

    private void OnEnable()
    {
        currentSelectableIndex = 0;
        selectableToTab[currentSelectableIndex].Select();
    }
}
