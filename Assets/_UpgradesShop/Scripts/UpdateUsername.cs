using System;
using TMPro;
using UnityEngine;

public class UpdateUsername : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_Text _usernameText;

    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void OnDoneButtonClick()
    {
        string newUsername = _usernameInputField.text;

        if (!String.IsNullOrWhiteSpace(newUsername))
        {
            PlayerPrefs.SetString("Username", newUsername);
        }

        _usernameText.SetText(PlayerPrefs.GetString("Username"));

        _usernameInputField.text = "";
        
        gameObject.SetActive(false);
    }
}