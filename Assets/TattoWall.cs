using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TattoWall : MonoBehaviour
{
    public List<Transform> FramePos = new List<Transform>();
    public Transform StartPos;
    public GameObject FramePrefab;
    public Texture[] SavedTattos;
    public int count;
    public TextMeshProUGUI valueText;

    [SerializeField] int i;
    private int _targetTattooValue;
    private float _currentTattooValue;
    private bool _shouldUpdateCash;
    private bool _isUnlockScreenEnabled;
    private float _incrementAmount;
    
    private void Awake()
    {
        count += 1;

        int totalEntered = PlayerPrefs.GetInt("totalEntered", 0);
        int No = totalEntered +count;

        PlayerPrefs.SetInt("totalEntered", No);
    }
   
    private void Start()
    {
        _currentTattooValue = StorageManager.GetTattooValue();
        valueText.SetText("$" + _currentTattooValue);
        
        _targetTattooValue = StorageManager.GetTattooValue();
        _targetTattooValue += StorageManager.Instance.RewardValue;
        
        _incrementAmount = (_targetTattooValue - _currentTattooValue) / 1.5f;
        
        StorageManager.SaveTattooValue(_targetTattooValue);
        
        EnableEndUi();

        StartCoroutine(Delay());
    }

    private void UpdateCash()
    {
        _shouldUpdateCash = true;
    }
    
    private void Update()
    {
        if (_shouldUpdateCash)
        {
            if (_currentTattooValue < _targetTattooValue)
            {
                _currentTattooValue += Time.unscaledDeltaTime * _incrementAmount;
                _currentTattooValue = Mathf.Clamp(_currentTattooValue, 0, _targetTattooValue);
                valueText.SetText("$" + Mathf.RoundToInt(_currentTattooValue));
            }
            else
            {
                if (!_isUnlockScreenEnabled)
                {
                    Invoke(nameof(EnableUnlockScreen), 2f);
                    _isUnlockScreenEnabled = false;
                }
            }
        }
    }

    private IEnumerator Delay()
    {
        i = PlayerPrefs.GetInt("totalEntered", 0);

        for (int j = 0; j < i; j++)
        {
            if (j != i - 1)
            {
                GameObject g = Instantiate(FramePrefab, FramePos[j].transform.position, Quaternion.identity);

                Texture2D m_TattoTex =(Texture2D)Resources.Load(PlayerPrefs.GetString("TattooFrame" + j));
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = m_TattoTex;
                g.transform.DOLocalRotate(new Vector3(0, -90, 0), 0);
            }
        }


        yield return new WaitForSeconds(1f);
        
        for (int j = 0; j < i; j++)
        {
            if (j == i - 1)
            {
                GameObject g = Instantiate(FramePrefab, StartPos.transform.position, Quaternion.identity);
                g.transform.DOLocalMove(FramePos[j].transform.position, 1.5f);
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameManager.Instance.LastTattoTexture;
                g.transform.DOLocalRotate(new Vector3(0, -90, 0), 0);
            }
        }
        
        Invoke(nameof(UpdateCash), 1.5f);
    }
    
    private void EnableEndUi()
    {
        if (GameManager.Instance.levelNo <= 3)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        }
        else if (GameManager.Instance.levelNo > 3)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 17;
        }
    }

    private void EnableUnlockScreen()
    {
        UiManager.Instance.UnlockPanel.gameObject.SetActive(true);
    }
}
