using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public GameObject logInPanel;
    public TMP_InputField logInNicknameInputField;
    public TMP_InputField logInPasswordInputField;
    public TMP_Text logInErrorMessageText;

    public GameObject signInPanel;
    public TMP_InputField signInNicknameInputField;
    public TMP_InputField signInPasswordInputField;
    public TMP_Text signInErrorMessageText;

    public GameObject mainMenuPanel;

    public void OnLogInClick()
    {
       var isAuthenticated = TotemManager.Instance.TotemMockDB.UsersDB
            .AuthenticateUser(logInNicknameInputField.text, logInPasswordInputField.text);

        if(isAuthenticated)
        {
            TotemManager.Instance.SetCurrentUser(logInNicknameInputField.text);
            ClosePanel(logInPanel, logInPasswordInputField, logInNicknameInputField, logInErrorMessageText);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            logInErrorMessageText.text = "Password and nickname are incorrect or user dose not exist.";
        }
    }

    public void OnSignInClick()
    {
        ClosePanel(logInPanel, logInPasswordInputField, logInNicknameInputField, logInErrorMessageText);
        signInPanel.SetActive(true);
    }

    public void OnSignInCancelClick()
    {
        CloseSignInPanel();
    }

    public void OnCreateNewAccountClick()
    {
        if(string.IsNullOrWhiteSpace(signInNicknameInputField.text))
        {
            signInErrorMessageText.text = "Your nickname can't be empty.";
            return;
        }
        if (signInPasswordInputField.text.Length < Constants.PASSWORD_MIN_LENGTH)
        {
            signInErrorMessageText.text = "Password length can't be less than 4.";
            return;
        }

        TotemManager.Instance.TotemMockDB.UsersDB
            .AddNewUser(signInNicknameInputField.text, signInPasswordInputField.text);
        CloseSignInPanel();
    }

    public void LogOut()
    {
        TotemManager.Instance.currentUser = null;
        logInPanel.SetActive(true);
    }

    private void ClosePanel(GameObject panel, TMP_InputField passwordInputField, 
        TMP_InputField nicknameInputField, TMP_Text errorMessage)
    {
        panel.SetActive(false);
        passwordInputField.text = string.Empty;
        nicknameInputField.text = string.Empty;
        errorMessage.text = string.Empty;
    }

    private void CloseSignInPanel()
    {
        ClosePanel(signInPanel, signInNicknameInputField, signInPasswordInputField, signInErrorMessageText);
        logInPanel.SetActive(true);
    }
}
