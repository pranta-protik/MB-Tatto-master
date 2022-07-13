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

    private AsyncOperation _mainSceneLoading;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        _time = 0f;
        LoadGame();
        // Invoke(nameof(LoadGame), second);
    }

    private void Update()
    {
        if (_time < second)
        {
            _time += Time.deltaTime;
            fillImage.fillAmount = _time / second;
        }
        
        if (_mainSceneLoading.isDone && _time >= second)
        {
            Destroy(this.gameObject);
        }
    }

    private void LoadGame()
    {
        _mainSceneLoading = SceneManager.LoadSceneAsync((int) SceneIndexes.MAIN);
    }
}
