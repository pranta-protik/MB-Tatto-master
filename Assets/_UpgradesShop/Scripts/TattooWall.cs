using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TattooWall : MonoBehaviour
{
    [SerializeField] private TMP_Text _usernameText;

    private void Start()
    {
        _usernameText.SetText(PlayerPrefs.GetString("Username"));
    }
}
