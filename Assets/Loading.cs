using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private float _time;

    public float second;

    public Image fillImage;
    
    
    void Start()
    {
        _time = 0f;
        Invoke("LoadGame", second);
    }

    private void Update()
    {
        if (_time < second)
        {
            _time += Time.deltaTime;
            fillImage.fillAmount = _time / second;
        }
    }

    public void LoadGame()
    {
        UiManager.Instance.CompleteUI.GetComponent<EndSceneUI>().Reset1();
    }
}
