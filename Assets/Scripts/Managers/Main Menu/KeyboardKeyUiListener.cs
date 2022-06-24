using UnityEngine;
using UnityEngine.UI;

public class KeyboardKeyUiListener : MonoBehaviour
{
    [SerializeField] private KeyCode keyboardCode;
    [SerializeField] private Button actionButton;

    private void Start()
    {
        if(actionButton == null)
        {
            actionButton = GetComponent<Button>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyboardCode))
        {
            actionButton.onClick.Invoke();
        }
    }
}
