using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class Book : MonoBehaviour
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

    public GameObject PageToFlip;
    public float waitTime = 1.5f;
    private void Awake()
    {
        count += 1;

        int totalEntered = PlayerPrefs.GetInt("totalEntered", 0);
        int No = totalEntered + count;

        PlayerPrefs.SetInt("totalEntered", No);
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        _currentTattooValue = StorageManager.GetTattooValue();
        valueText.SetText("$" + _currentTattooValue);

        _targetTattooValue = StorageManager.GetTattooValue();
        _targetTattooValue += StorageManager.Instance.RewardValue;

        _incrementAmount = (_targetTattooValue - _currentTattooValue) / 1.5f;

        StorageManager.SaveTattooValue(_targetTattooValue);

        EnableEndUi();

        StartCoroutine(Delay());
    }
    private void Start()
    {
        StartCoroutine(Wait());
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
    int j;
    private IEnumerator Delay()
    {
       
           i = PlayerPrefs.GetInt("totalEntered", 0);
 
        for ( j = 0; j < i; j++)
        {
            if (j != i - 1)
            {
          
                GameObject g = Instantiate(FramePrefab, FramePos[j].transform.position, Quaternion.identity);
                g.transform.parent = PageToFlip.transform;
                g.transform.DOLocalRotate(new Vector3(0, 0, 0), 0);
                g.transform.DOLocalMove(new Vector3(0, 0, 0), 0);
               
                Texture2D m_TattoTex = (Texture2D)Resources.Load(PlayerPrefs.GetString("TattooFrame" + j));
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = m_TattoTex;
                
            }
        }


        yield return new WaitForSeconds(1f);
        PageToFlip.transform.DOLocalRotate(new Vector3(90f, 90f, -90f), 1.3f);

        yield return new WaitForSeconds(2f);

        for (int j = 0; j < i; j++)
        {
            if (j == i - 1)
            {
                GameObject g = Instantiate(FramePrefab, StartPos.transform.position, Quaternion.identity);
                g.transform.DOLocalMove(FramePos[j].transform.position, 1.5f);
                g.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameManager.Instance.LastTattoTexture;
                g.transform.DOLocalRotate(new Vector3(-50f, -90f, -0.791f), 0);
            }
        }

        Invoke(nameof(UpdateCash), 1.5f);
    }

    private void EnableEndUi()
    {
        if (GameManager.Instance.levelNo == 0)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 50;
        }
        else if (GameManager.Instance.levelNo == 1)
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 50;
        }
        else
        {
            UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 33;
        }
    }

    private void EnableUnlockScreen()
    {
        UiManager.Instance.UnlockPanel.gameObject.SetActive(true);
    }
}
